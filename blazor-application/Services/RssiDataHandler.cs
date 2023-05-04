using BlazorBLE.Data;

namespace BlazorBLE.Services;

public sealed class RssiDataHandler
{
    private readonly KnnClassifier classifier;
    
    private RssiDataSet dataSet;

    public RssiDataHandler(int k, double threshold)
    {
        classifier = new KnnClassifier(k, threshold);
    }

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
    
    public bool IsInsideRoom(RawBeaconRssiMeasurement rawBeaconRssiMeasurement)
    {
        if (dataSet?.RssiDataPoints == null) return false;
        if (dataSet.RssiDataPoints.Count == 0) return false;
        if (rawBeaconRssiMeasurement.Count != dataSet.NumBeacons) return false;

        return classifier.Classify(rawBeaconRssiMeasurement, dataSet.RssiDataPoints) == ClassLabel.Inside;
    }
}