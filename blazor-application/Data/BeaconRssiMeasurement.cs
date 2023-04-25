namespace BlazorBLE.Data;

public readonly struct BeaconRssiMeasurement
{
    public Guid BeaconId { get; }
    
    public int Rssi { get; }
    
    public BeaconRssiMeasurement(Guid beaconId, int rssi)
    {
        BeaconId = beaconId;
        Rssi = rssi;
    }
}