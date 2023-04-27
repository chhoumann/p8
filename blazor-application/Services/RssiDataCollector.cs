using BlazorBLE.Data;
using Plugin.BLE.Abstractions.Contracts;

namespace BlazorBLE.Services;

public sealed class RssiDataCollector
{
    public event Action BeaconRssisUpdated;

    public ClassLabel CurrentLabel { get; set; }
    
    public RssiDataSet DataSet { get; private set; }

    public bool IsListening { get; private set; }
    public bool IsCollecting { get; private set; }

    private PeriodicTimer periodicTimer;
    private Guid[] beaconGuids;
    private Dictionary<Guid, int> beaconRssis;
    private CancellationTokenSource cts;

    public void Listen(IReadOnlyList<IDevice> beacons, TimeSpan interval)
    {
        if (IsListening) return;

        DataSet = new RssiDataSet(beacons);
        periodicTimer = new PeriodicTimer(interval);
        beaconRssis = new Dictionary<Guid, int>();
        beaconGuids = new Guid[beacons.Count];

        for (int i = 0; i < beaconGuids.Length; i++)
        {
            IDevice beacon = beacons[i];

            beaconRssis.Add(beacon.Id, beacon.Rssi);
            beaconGuids[i] = beacon.Id;
        }

        IsListening = true;
    }
    
    public void StopListening()
    {
        if (!IsListening) return;

        periodicTimer.Dispose();
        IsListening = false;
        
        if (IsCollecting)
        {
            StopCollectingRssiData();
        }
    }

    public void CollectRssiData()
    {
        if (!IsListening) return;
        if (IsCollecting) return;        

        cts = new CancellationTokenSource();
        new SoundEffect("StartCollectingData.mp3").Play();
        
        Task.Run(async () =>
        {
            RssiLowVarianceSample sample = new(beaconGuids);
            
            while (await periodicTimer.WaitForNextTickAsync() && IsCollecting)
            {
                sample.Add(GetLatestMeasurement());
                
                if (sample.IsStable(5))
                {
                    DataSet.Add(sample.CalculateAverageMeasurement(), CurrentLabel);
                    StopCollectingRssiData();
                }
            }
        }, cts.Token);
        
        IsCollecting = true;
    }

    public void StopCollectingRssiData()
    {
        if (!IsCollecting) return;

        cts.Cancel();
        IsCollecting = false;
        
        new SoundEffect("StopCollectingData.mp3").Play();
    }

    public void UpdateBeaconRssi(IDevice device)
    {
        if (!IsListening) return;
        if (!beaconRssis.ContainsKey(device.Id)) return;

        beaconRssis[device.Id] = device.Rssi;

        BeaconRssisUpdated?.Invoke();
    }

    public BeaconRssiMeasurement<int> GetLatestMeasurement()
    {
        BeaconRssiMeasurement<int> measurement = new(beaconGuids.Length);
        
        for (int i = 0; i < measurement.Count; i++)
        {
            Guid guid = beaconGuids[i];
            int rssi = beaconRssis[guid];
            
            measurement.Add(guid, rssi);
        }
        
        return measurement;
    }
}