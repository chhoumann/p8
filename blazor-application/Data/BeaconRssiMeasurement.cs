namespace BlazorBLE.Data;

public readonly struct BeaconRssiMeasurement
{
    public readonly Guid BeaconId;
    public readonly int Rssi;

    public BeaconRssiMeasurement(Guid beaconId, int rssi)
    {
        BeaconId = beaconId;
        Rssi = rssi;
    }
}