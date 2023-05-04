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
    
    public bool IsInsideRoom(RawBeaconRssiMeasurement beaconMeasurements, double distanceThreshold)
    {
        if (dataSet == null)
        {
            return false;
        }
        
        if (beaconMeasurements.Count != dataSet.NumBeacons)
        {
            return false;
        }

        const double threshold = 0.5;
        const int k = 5;

        // Get the distance between the beacon measurements and each data point 
        // Find the K nearest neighbors to the beacon measurement provided in the argument
        // If the majority of the K nearest neighbors are inside the room, return true
        
        DataPointDistance[] distances = new DataPointDistance[dataSet.RssiDataPoints.Count];

        for (int i = 0; i < dataSet.RssiDataPoints.Count; i++)
        {
            DataPoint dataPoint = dataSet.RssiDataPoints[i];
            double distance = dataPoint.Distance(beaconMeasurements.Rssis);
            
            distances[i] = new DataPointDistance(dataPoint, distance);
        }

        // Get the 5 lowest distances from the dictionary (the 5 nearest neighbors)
        DataPointDistance[] kNearestNeighbors = GetKNearestNeighbors(distances, k);
        
        // Calculate the sum of the inverse distances of all neighbors
        double sumOfInverseDistances = kNearestNeighbors.Sum(neighbor => 1 / neighbor.Distance);

        // Count the number of neighbors that are inside the room and weigh them by their distance
        double weightedNumNeighborsInsideRoom = 0;

        foreach (DataPointDistance neighbor in kNearestNeighbors)
        {
            if (neighbor.DataPoint.Label == ClassLabel.Inside)
            {
                weightedNumNeighborsInsideRoom += (1 / neighbor.Distance) / sumOfInverseDistances;
            }
        }

        // If the probability of the data point being inside the room is greater than the threshold, return true
        return weightedNumNeighborsInsideRoom > threshold;
    }

    private static DataPointDistance[] GetKNearestNeighbors(DataPointDistance[] distances, int k = 5)
    {
        DataPointDistance[] result = new DataPointDistance[k];
        Random random = new();

        QuickSelect(distances, 0, distances.Length - 1, k, random);

        Array.Copy(distances, result, k);
        
        return result;
    }

    private static void QuickSelect(DataPointDistance[] distances, int left, int right, int k, Random random)
    {
        if (left == right) return;

        int pivotIndex = random.Next(left, right);
        pivotIndex = Partition(distances, left, right, pivotIndex);

        if (k == pivotIndex) return;

        if (k < pivotIndex)
        {
            QuickSelect(distances, left, pivotIndex - 1, k, random);
        }
        else
        {
            QuickSelect(distances, pivotIndex + 1, right, k, random);
        }
    }

    private static int Partition(IList<DataPointDistance> distances, int left, int right, int pivotIndex)
    {
        DataPointDistance pivotValue = distances[pivotIndex];
        
        Swap(distances, pivotIndex, right);

        int storeIndex = left;
        
        for (int i = left; i < right; i++)
        {
            if (distances[i].Distance < pivotValue.Distance)
            {
                Swap(distances, storeIndex, i);
                storeIndex++;
            }
        }

        Swap(distances, right, storeIndex);
        
        return storeIndex;
    }

    private static void Swap(IList<DataPointDistance> distances, int a, int b)
    {
        (distances[a], distances[b]) = (distances[b], distances[a]);
    }
    
    private sealed class DataPointDistance
    {
        public DataPoint DataPoint { get; }
        
        public double Distance { get; }
        
        public DataPointDistance(DataPoint dataPoint, double distance)
        {
            DataPoint = dataPoint;
            Distance = distance;
        }
    }
}