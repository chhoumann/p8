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

        const int k = 5;

        // double[] distances = new double[dataSet.RssiDataPoints.Count];
        Dictionary<DataPoint, double> distances = new(dataSet.RssiDataPoints.Count);

        for (int i = 0; i < dataSet.RssiDataPoints.Count; i++)
        {
            var dataPoint = dataSet.RssiDataPoints[i];
            
        }

        // GetKNearestNeighbors(distances, k);

        return false;
    }

    private static double[] GetKNearestNeighbors(IReadOnlyList<double> distances, int k = 5)
    {
        double[] kSmallestDistances = new double[k];

        for (int i = 0; i < k; i++)
        {
            kSmallestDistances[i] = double.MaxValue;
        }

        for (int i = 0; i < distances.Count; i++)
        {
            double distance = distances[i];

            for (int j = 0; j < k; j++)
            {
                if (distance < kSmallestDistances[j])
                {
                    for (int n = k - 1; n > j; n--)
                    {
                        kSmallestDistances[n] = kSmallestDistances[n - 1];
                    }

                    kSmallestDistances[j] = distance;
                    break;
                }
            }
        }
        
        return kSmallestDistances;
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