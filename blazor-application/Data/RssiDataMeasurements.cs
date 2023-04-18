namespace BlazorBLE.Data;

public sealed class RssiDataMeasurements
{
    public int NumBeacons { get; set; }

    public List<int[]> RssiDataPoints { get; set; }

    public RssiDataMeasurements(int numBeacons)
    {
        NumBeacons = numBeacons;
        RssiDataPoints = new List<int[]>();
    }

    public void Add(int[] measurement)
    {
        if (measurement.Length != NumBeacons)
        {
            throw new ArgumentException($"Size of measurement ({measurement.Length}) must be equal to the number of beacons ({NumBeacons}).");
        }

        RssiDataPoints.Add(measurement);
    }
}
