namespace BlazorBLE.Data;

public sealed class BeaconRssiMeasurement
{
    public Guid[] BeaconIds { get; }
    public int[] Rssis { get; }

    public int Count { get; }

    private int index;

    public BeaconRssiMeasurement(int count)
    {
        Count = count;
        BeaconIds = new Guid[count];
        Rssis = new int[count];
    }
    
    public void Add(Guid beaconId, int rssi)
    {
        BeaconIds[index] = beaconId;
        Rssis[index] = rssi;
        index++;
    }
}