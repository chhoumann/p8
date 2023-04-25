using BlazorBLE.Data;
using Plugin.BLE.Abstractions.Contracts;

namespace BlazorBLE.Services;

public sealed class RssiDataCollector
{
    public event Action BeaconRssisUpdated;

    public ClassLabel CurrentLabel { get; set; }
    
    public RssiDataSet DataSet { get; private set; }

    public bool IsMeasuring { get; private set; }
    public bool IsCollecting { get; private set; }

    private PeriodicTimer periodicTimer;
    private Guid[] beaconGuids;
    private Dictionary<Guid, int> beaconRssis;
    private CancellationTokenSource cts;

    public void StartMeasuring(IReadOnlyList<IDevice> beacons, TimeSpan interval)
    {
        if (IsMeasuring) return;

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

        IsMeasuring = true;
    }
    
    public void StopMeasuring()
    {
        if (!IsMeasuring) return;

        periodicTimer.Dispose();
        IsMeasuring = false;
        
        if (IsCollecting)
        {
            StopCollectingRssiData();
        }
    }

    public void CollectRssiData()
    {
        if (!IsMeasuring) return;
        if (IsCollecting) return;

        cts = new CancellationTokenSource();

        Task.Run(async () =>
        {
            while (await periodicTimer.WaitForNextTickAsync() && IsCollecting)
            {
                DataSet.Add(GetLatestDataPoint());
            }
        }, cts.Token);
        
        IsCollecting = true;
    }

    public void StopCollectingRssiData()
    {
        if (!IsCollecting) return;

        cts.Cancel();
        IsCollecting = false;
    }

    public void UpdateBeaconRssi(IDevice device)
    {
        if (!IsMeasuring) return;
        if (!beaconRssis.ContainsKey(device.Id)) return;

        beaconRssis[device.Id] = device.Rssi;

        BeaconRssisUpdated?.Invoke();
    }

    public BeaconRssiMeasurement[] GetLatestDataPoint()
    {
        BeaconRssiMeasurement[] measurements = new BeaconRssiMeasurement[beaconGuids.Length];
        
        for (int i = 0; i < measurements.Length; i++)
        {
            Guid guid = beaconGuids[i];
            int rssi = beaconRssis[guid];
            
            measurements[i] = new BeaconRssiMeasurement(guid, rssi, CurrentLabel);
        }
        
        return measurements;
    }
}