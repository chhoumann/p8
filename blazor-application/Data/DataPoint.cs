namespace BlazorBLE.Data;

public readonly struct DataPoint
{
    public readonly int Rssi;
    public readonly ClassLabel Label;
    
    public DataPoint(int rssi, ClassLabel label)
    {
        Rssi = rssi;
        Label = label;
    }
}