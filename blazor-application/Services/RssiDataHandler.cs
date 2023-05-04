using BlazorBLE.Data;

namespace BlazorBLE.Services;

public sealed class RssiDataHandler
{
    public KnnClassifier Classifier { get; }
    
    private RssiDataSet dataSet;

    public RssiDataHandler(int k, double threshold)
    {
        Classifier = new KnnClassifier(k, threshold);
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
        if (rawBeaconRssiMeasurement.Count != dataSet.NumBeacons) return false;

        return Classifier.Classify(rawBeaconRssiMeasurement, dataSet.RssiDataPoints) == ClassLabel.Inside;
    }
}