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
    
    public bool IsInsideRoom(BeaconRssiMeasurement beaconMeasurements, double distanceThreshold)
    {
        if (dataSet == null)
        {
            return false;
        }
        
        if (beaconMeasurements.Count != dataSet.NumBeacons)
        {
            return false;
        }

        for (int i = 0; i < dataSet.RssiDataPoints.Count; i++)
        {
            DataPoint dataPoint = dataSet.RssiDataPoints[i];

            double distance = DataPoint.Distance(dataPoint, beaconMeasurements.Rssis);
            
            if (distance < distanceThreshold)
            {
                return true;
            }
        }

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
}