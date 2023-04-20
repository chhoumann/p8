using BlazorBLE.Data;
using Plugin.BLE.Abstractions.Contracts;

namespace BlazorBLE.Services;

public sealed class RssiDataCollector
{
    public event Action BeaconRssisUpdated;

    public RssiDataMeasurements Measurements { get; private set; }

    public bool IsMeasuring { get; private set; }
    public bool IsCollecting { get; private set; }

    private PeriodicTimer periodicTimer;
    private Guid[] beaconGuids;
    private Dictionary<Guid, int> beaconRssis;
    private CancellationTokenSource cts;

    public void StartMeasuring(IReadOnlyList<IDevice> beacons, TimeSpan interval)
    {
        if (IsMeasuring) return;

        Measurements = new RssiDataMeasurements(beacons.Count);
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
        if (!IsMeasuring)
        if (IsCollecting) return;

        cts = new CancellationTokenSource();

        Task.Run(async () =>
        {
            while (await periodicTimer.WaitForNextTickAsync() && IsCollecting)
            {
                Measurements.Add(GetLatestMeasurement());
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

    public int[] GetLatestMeasurement()
    {
        int[] rssiMeasurement = new int[beaconGuids.Length];

        for (int i = 0; i < beaconGuids.Length; i++)
        {
            rssiMeasurement[i] = beaconRssis[beaconGuids[i]];
        }

        return rssiMeasurement;
    }
}