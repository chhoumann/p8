using System.Globalization;

namespace BlazorBLE.Data;

public sealed class DataPoint<T> where T : struct, IConvertible
{
    public ClassLabel Label { get; }
    
    public T[] Rssis { get; }

    public DataPoint(ClassLabel label, T[] rssis)
    {
        Label = label;
        Rssis = rssis;
    }

    public double DistanceTo(int[] rssis)
    {
        if (rssis.Length != Rssis.Length)
        {
            throw new ArgumentException("The number of RSSI values must be the same.");
        }
        
        double distanceSquared = 0;
        
        for (int i = 0; i < Rssis.Length; i++)
        {
            double difference = ToDouble(Rssis[i]) - rssis[i];
            distanceSquared += difference * difference;
        }
        
        return Math.Sqrt(distanceSquared);
    }
    
    /// <summary>
    /// Helper method to convert a value to a double type while preventing boxing (like Convert.ToDouble() does).
    /// </summary>
    private static double ToDouble<V>(V value) where V : struct, IConvertible
    {
        return value.ToDouble(CultureInfo.InvariantCulture);
    }
}