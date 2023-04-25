namespace BlazorBLE.Services;

public static class BLEMath
{
    public static double LogDistancePathLoss(int rssi, int txPower, double n)
    {
        double exp = (txPower - rssi) / (10 * n);
        double distance = Math.Pow(10, exp);

        return distance;
    }
}