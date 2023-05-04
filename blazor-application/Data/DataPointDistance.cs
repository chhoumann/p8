namespace BlazorBLE.Data;

public sealed class DataPointDistance
{
    public DataPoint DataPoint { get; }
        
    public double Distance { get; }
        
    public DataPointDistance(DataPoint dataPoint, double distance)
    {
        DataPoint = dataPoint;
        Distance = distance;
    }
}