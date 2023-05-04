namespace BlazorBLE.Data;

public sealed class RawBeaconRssiMeasurement : BeaconRssiMeasurement<int>
{
    public RawBeaconRssiMeasurement(int count) : base(count) { }

    public RawBeaconRssiMeasurement(Guid[] beaconIds, int[] rssis) : base(beaconIds, rssis) { }
}

public sealed class AveragedBeaconRssiMeasurement : BeaconRssiMeasurement<double>
{
    public AveragedBeaconRssiMeasurement(int count) : base(count) { }
    
    public AveragedBeaconRssiMeasurement(Guid[] beaconIds, double[] rssis) : base(beaconIds, rssis) { }
}

public abstract class BeaconRssiMeasurement<T>
{
    public Guid[] BeaconIds { get; }
    public T[] Rssis { get; }

    public int Count { get; }

    private int index;

    public BeaconRssiMeasurement(int count)
    {
        Count = count;
        BeaconIds = new Guid[count];
        Rssis = new T[count];
    }
    
    public BeaconRssiMeasurement(Guid[] beaconIds, T[] rssis)
    {
        if (beaconIds.Length != rssis.Length)
        {
            throw new ArgumentException("Beacon IDs and RSSIs must be the same length.");
        }
        
        BeaconIds = beaconIds;
        Rssis = rssis;
        Count = beaconIds.Length;
    }
    
    public void Add(Guid beaconId, T rssi)
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