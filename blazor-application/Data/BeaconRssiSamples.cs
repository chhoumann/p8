namespace BlazorBLE.Data;

public sealed class BeaconRssiSamples
{
    public int Count => rssis.Count;
    
    private readonly List<int> rssis = new();
    
    private bool isStable;

    public void Add(int rssi) => rssis.Add(rssi);

    public bool IsStable(double threshold)
    {
        if (isStable) return true;
        
        double standardDeviation = CalculateStandardDeviation();
        
        isStable = standardDeviation < threshold;

        return isStable;
    }

    public double Average() => rssis.Average();

    private double CalculateStandardDeviation()
    {
        // Calculate the mean RSSI for each beacon
        double meanRssi = rssis.Average();

        // Calculate the variance
        double variance = rssis.Sum(rssi => Math.Pow(rssi - meanRssi, 2)) / rssis.Count;

        // Calculate the standard deviation
        return Math.Sqrt(variance);
    }
}