using BlazorBLE.Data;

namespace BlazorBLE.Services;

public sealed class KnnClassifier
{
    public int K { get; set; }
    
    public double Threshold { get; set; }
    public double WeightedNumNeighborsInsideRoom { get; private set; }

    public KnnClassifier(int k, double threshold)
    {
        K = k;
        Threshold = threshold;
    }
    
    public ClassLabel Classify(RawBeaconRssiMeasurement rawBeaconRssiMeasurement, IReadOnlyList<DataPoint> rssiDataPoints)
    {
        return Classify(rawBeaconRssiMeasurement.Rssis, rssiDataPoints);
    }

    public ClassLabel Classify(int[] rssis, IReadOnlyList<DataPoint> rssiDataPoints)
    {
        if (K > rssis.Length) return ClassLabel.Outside;
        
        DataPointDistance[] distances = CalculateDistances(rssis, rssiDataPoints);
        DataPointDistance[] kNearestNeighbors = GetKNearestNeighbors(distances, K);

        WeightedNumNeighborsInsideRoom = CalculateWeight(kNearestNeighbors, ClassLabel.Inside);

        return WeightedNumNeighborsInsideRoom > Threshold ? ClassLabel.Inside : ClassLabel.Outside;
    }

    private static DataPointDistance[] CalculateDistances(int[] rssis, IReadOnlyList<DataPoint> rssiDataPoints)
    {
        DataPointDistance[] distances = new DataPointDistance[rssiDataPoints.Count];

        for (int i = 0; i < rssiDataPoints.Count; i++)
        {
            DataPoint dataPoint = rssiDataPoints[i];
            double distance = dataPoint.Distance(rssis);

            distances[i] = new DataPointDistance(dataPoint, distance);
        }

        return distances;
    }
    
    private static double CalculateWeight(IReadOnlyList<DataPointDistance> kNearestNeighbors, ClassLabel label)
    {
        double sumOfInverseDistances = 0;

        for (int i = 0; i < kNearestNeighbors.Count; i++)
        {
            sumOfInverseDistances += 1 / kNearestNeighbors[i].Distance;
        }

        // Count the number of neighbors that are inside the room and weigh them by their distance
        double weightedNumNeighborsInsideRoom = 0;

        for (int i = 0; i < kNearestNeighbors.Count; i++)
        {
            if (kNearestNeighbors[i].DataPoint.Label == label)
            {
                weightedNumNeighborsInsideRoom += (1 / kNearestNeighbors[i].Distance) / sumOfInverseDistances;
            }
        }

        return weightedNumNeighborsInsideRoom;
    }

    private static DataPointDistance[] GetKNearestNeighbors(DataPointDistance[] distances, int k)
    {
        DataPointDistance[] result = new DataPointDistance[k];
        Random random = new();

        QuickSelect(distances, 0, distances.Length - 1, k, random);

        Array.Copy(distances, result, k);
        
        return result;
    }

    private static void QuickSelect(IList<DataPointDistance> distances, int left, int right, int k, Random random)
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
}