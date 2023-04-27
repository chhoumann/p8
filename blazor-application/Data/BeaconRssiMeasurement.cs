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
    
    public BeaconRssiMeasurement(Guid[] beaconIds, int[] rssis)
    {
        if (beaconIds.Length != rssis.Length)
        {
            throw new ArgumentException("Beacon IDs and RSSIs must be the same length.");
        }
        
        BeaconIds = beaconIds;
        Rssis = rssis;
        Count = beaconIds.Length;
    }
    
    public void Add(Guid beaconId, int rssi)
    {
        BeaconIds[index] = beaconId;
        Rssis[index] = rssi;
        index++;
    }

    public override string ToString()
    {
        return $"Beacon RSSIs: {string.Join(", ", Rssis)}";
    }
}