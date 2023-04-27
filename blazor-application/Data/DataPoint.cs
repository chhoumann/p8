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

    public static double Distance(DataPoint p1, DataPoint p2) => Distance(p1.Rssis, p2.Rssis);

    public static double Distance(double[] rssis, DataPoint p) => Distance(rssis, p.Rssis);
    
    public static double Distance(DataPoint p, double[] rssis) => Distance(p.Rssis, rssis);

    public static double Distance(double[] rssis1, double[] rssis2)
    {
        if (rssis1.Length != rssis2.Length)
        {
            throw new ArgumentException("The number of RSSI values must be the same.");
        }
        
        double distanceSquared = 0;
        
        for (int i = 0; i < rssis1.Length; i++)
        {
            double difference = rssis1[i] - rssis2[i];
            distanceSquared += difference * difference;
        }
        
        return Math.Sqrt(distanceSquared);
    }
}