using Newtonsoft.Json;

namespace BlazorBLE.Data;

public sealed class RssiDataMeasurements
{
    public int NumBeacons { get; }

    public List<int[]> RssiDataPoints { get; }

    public RssiDataMeasurements(int numBeacons)
    {
        NumBeacons = numBeacons;
        RssiDataPoints = new List<int[]>();
    }

    public static RssiDataMeasurements ReadFromJson(string jsonFilePath)
    {
        string jsonString = File.ReadAllText(jsonFilePath);
        
        return JsonConvert.DeserializeObject<RssiDataMeasurements>(jsonString);
    }

    public void WriteToJson(string fileName)
    {
        string jsonString = JsonConvert.SerializeObject(this);
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);
        
        File.WriteAllText(filePath, jsonString);
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