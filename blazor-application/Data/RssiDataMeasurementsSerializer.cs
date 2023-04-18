using Newtonsoft.Json;

namespace BlazorBLE.Data;

public static class RssiDataMeasurementsSerializer
{
    public static string Serialize(RssiDataMeasurements dataMeasurements)
    {
        return JsonConvert.SerializeObject(dataMeasurements);
    }

    public static RssiDataMeasurements Deserialize(string json)
    {
        return JsonConvert.DeserializeObject<RssiDataMeasurements>(json);
    }
    
    public static void ToFile(RssiDataMeasurements dataMeasurements, string filePath)
    {
        File.WriteAllText(filePath, Serialize(dataMeasurements));
    }
    
    public static RssiDataMeasurements FromFile(string filePath)
    {
        return Deserialize(File.ReadAllText(filePath));
    }
}