namespace BlazorBLE.Services;

public static class BLEMath
{
    public static double[] ToMeters(int[] rssis, int txPower, double n)
    {
        double[] distances = new double[rssis.Length];

        for (int i = 0; i < rssis.Length; i++)
        {
            int rssi = rssis[i];
                
            distances[i] = LogDistancePathLoss(rssi, txPower, n);
        }

        return distances;
    }

    public static double LogDistancePathLoss(int rssi, int txPower, double n)
    {
        double exp = (txPower - rssi) / (10 * n);
        double distance = Math.Pow(10, exp);

        return distance;
    }
}