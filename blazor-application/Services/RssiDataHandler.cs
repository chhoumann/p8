using BlazorBLE.Data;

namespace BlazorBLE.Services;

public sealed class RssiDataHandler
{
    private RssiDataSet dataSet;

    public void LoadData(string jsonFileName)
    {
        try
        {
            dataSet = RssiDataSet.ReadFromJson(jsonFileName);
            dataSet.RemoveDuplicates();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public bool IsInsideRoom(BeaconRssiMeasurement[] beaconMeasurements, double distanceThreshold)
    {
        if (dataSet == null)
        {
            return false;
        }
        
        if (beaconMeasurements.Length != dataSet.NumBeacons)
        {
            return false;
        }
        
        int[] rssis = beaconMeasurements.Select(m => m.Rssi).ToArray();

        foreach (int[] dataPoint in dataSet.RssiDataPoints)
        {
            double dist = GetDistance(dataPoint, rssis);
            
            if (dist < distanceThreshold)
            {
                return true;
            }
        }

        return false;
    }

    private static double GetDistance(IReadOnlyList<int> vector1, IReadOnlyList<int> vector2)
    {
        double distanceSquared = 0;

        for (int i = 0; i < vector1.Count; i++)
        {
            double difference = vector1[i] - vector2[i];
            distanceSquared += difference * difference;
        }

        return Math.Sqrt(distanceSquared);
    }
}