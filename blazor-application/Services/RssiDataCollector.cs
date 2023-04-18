using BlazorBLE.Data;
using Plugin.BLE.Abstractions.Contracts;

namespace BlazorBLE.Services;

public sealed class RssiDataCollector
{
    public bool IsMeasuring { get; private set; }

    private RssiDataMeasurements dataMeasurements;
    private PeriodicTimer periodicTimer;
    private Guid[] beaconGuids;
    private Dictionary<Guid, int> beaconRssis;

    public void StartMeasuring(IReadOnlyList<IDevice> beacons, TimeSpan interval)
    {
        if (IsMeasuring) return;

        dataMeasurements = new RssiDataMeasurements(beacons.Count);
        periodicTimer = new PeriodicTimer(interval);
        beaconRssis = new Dictionary<Guid, int>();
        beaconGuids = new Guid[beacons.Count];

        for (int i = 0; i < beaconGuids.Length; i++)
        {
            IDevice beacon = beacons[i];

            beaconRssis.Add(beacon.Id, beacon.Rssi);
            beaconGuids[i] = beacon.Id;
        }

        Task.Run(CollectRssiData);

        IsMeasuring = true;
    }

    public void StopMeasuring()
    {
        if (!IsMeasuring) return;

        periodicTimer.Dispose();
        IsMeasuring = false;
    }

    public void UpdateBeaconRssi(IDevice device)
    {
        if (!IsMeasuring) return;
        if (!beaconRssis.ContainsKey(device.Id)) return;

        beaconRssis[device.Id] = device.Rssi;
    }

    private async Task CollectRssiData()
    {
        while (await periodicTimer.WaitForNextTickAsync())
        {
            int[] rssiMeasurement = new int[beaconGuids.Length];

            for (int i = 0; i < beaconGuids.Length; i++)
            {
                rssiMeasurement[i] = beaconRssis[beaconGuids[i]];
            }
            
            dataMeasurements.Add(rssiMeasurement);
        }
    }
}