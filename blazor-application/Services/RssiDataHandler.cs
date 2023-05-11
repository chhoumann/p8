﻿using BlazorBLE.Data;

namespace BlazorBLE.Services;

public sealed class RssiDataHandler
{
    public KnnClassifier Classifier { get; }
    
    private RssiDataSet dataSet;

    public RssiDataHandler(int k, double threshold)
    {
        Classifier = new KnnClassifier(k, threshold);
    }

    public async void LoadData(string jsonFileName)
    {
        try
        {
            dataSet = await RssiDataSet.ReadFromJson(jsonFileName);
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

        return Classifier.Classify(rawBeaconRssiMeasurement, dataSet.RssiDataPoints) == ClassLabel.Inside;
    }
}