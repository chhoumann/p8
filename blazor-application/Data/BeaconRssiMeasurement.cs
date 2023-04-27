namespace BlazorBLE.Data;

public struct BeaconRssiMeasurement
{
    public readonly Guid[] BeaconIds;
    public readonly int[] Rssis;

    public readonly int Count;

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