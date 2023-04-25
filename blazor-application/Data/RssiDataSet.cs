using System.Text;
using Newtonsoft.Json;
using Plugin.BLE.Abstractions.Contracts;

namespace BlazorBLE.Data;

public sealed class RssiDataSet
{
    public int NumBeacons { get; set; }

    public Guid[] BeaconIds { get; set; }
    
    public List<int[]> RssiDataPoints { get; set; }
    
    [JsonConstructor]
    public RssiDataSet() { }
    
    public RssiDataSet(IReadOnlyList<IDevice> beacons)
    {
        NumBeacons = beacons.Count;
        RssiDataPoints = new();
        BeaconIds = new Guid[NumBeacons];
        
        for (int i = 0; i < NumBeacons; i++)
        {
            BeaconIds[i] = beacons[i].Id;
        }
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

    public void Add(BeaconRssiMeasurement[] measurement)
    {
        if (measurement.Length != NumBeacons)
        {
            throw new ArgumentException("Measurement must have the same number of elements as the number of beacons.");
        }

        RssiDataPoints.Add(new int[NumBeacons]);
        
        for (int i = 0; i < measurement.Length; i++)
        {
            Guid beaconId = measurement[i].BeaconId;
            
            if (BeaconIds[i] == beaconId)
            {
                RssiDataPoints[^1][i] = measurement[i].Rssi;
            }
        }
    }

    public void RemoveDuplicates()
    {
        RssiDataPoints = RssiDataPoints.GroupBy(c => string.Join(",", c))
                    .Select(c => c.First().ToArray()).ToList();
    }
    
    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"Number of beacons: {NumBeacons}")
            .AppendLine($"Number of data points: {RssiDataPoints?.Count ?? 0}")
            .ToString();
    }

    private static string GetPath(string fileName)
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);
    }
}