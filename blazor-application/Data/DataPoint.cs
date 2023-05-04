namespace BlazorBLE.Data;

public sealed class DataPoint 
{
    public ClassLabel Label { get; }
    
    public double[] Rssis { get; }

    public DataPoint(ClassLabel label, double[] rssis)
    {
        Label = label;
        Rssis = rssis;
    }

    public double Distance(int[] rssis)
    {
        if (rssis.Length != Rssis.Length)
        {
            throw new ArgumentException("The number of RSSI values must be the same.");
        }
        
        double distanceSquared = 0;
        
        for (int i = 0; i < Rssis.Length; i++)
        {
            double difference = Rssis[i] - rssis[i];
            distanceSquared += difference * difference;
        }
        
        return Math.Sqrt(distanceSquared);
    }
}