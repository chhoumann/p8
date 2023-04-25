namespace BlazorBLE.Data;

public readonly struct BeaconRssiMeasurement
{
    public readonly Guid BeaconId;
    public readonly ClassLabel Label;

    public readonly int Rssi; 
    
    public BeaconRssiMeasurement(Guid beaconId, int rssi, ClassLabel label)
    {
        BeaconId = beaconId;
        Rssi = rssi;
        Label = label;
    }
}