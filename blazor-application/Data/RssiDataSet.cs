using BlazorBLE.Services;
using Newtonsoft.Json;

namespace BlazorBLE.Data;

public sealed class RssiDataSet
{
    public int NumBeacons { get; }

    public List<int[]> RssiDataPoints { get; private set; }

    public RssiDataSet(int numBeacons)
    {
        NumBeacons = numBeacons;
        RssiDataPoints = new List<int[]>();
    }

    public static RssiDataSet ReadFromJson(string fileName)
    {
        string jsonString = File.ReadAllText(GetPath(fileName));
        
        return JsonConvert.DeserializeObject<RssiDataSet>(jsonString);
    }

    public void WriteToJson(string fileName)
    {
        string jsonString = JsonConvert.SerializeObject(this);
        
        File.WriteAllText(GetPath(fileName), jsonString);
    }

    public void Add(int[] measurement)
    {
        if (measurement.Length != NumBeacons)
        {
            throw new ArgumentException($"Size of measurement ({measurement.Length}) must be equal to the number of beacons ({NumBeacons}).");
        }

        RssiDataPoints.Add(measurement);
    }

    public void RemoveDuplicates()
    {
        RssiDataPoints = RssiDataPoints.GroupBy(c => string.Join(",", c))
            .Select(c => c.First().ToArray()).ToList();
    }

    public List<double[]> ToMeters(int txPower, double n)
    {
        List<double[]> dataPointsInMeters = new();
        
        foreach (int[] measurement in RssiDataPoints)
        {
            dataPointsInMeters.Add(BLEMath.ToMeters(measurement, txPower, n));
        }
        
        return dataPointsInMeters;
    }

    private static string GetPath(string fileName)
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);
    }
}