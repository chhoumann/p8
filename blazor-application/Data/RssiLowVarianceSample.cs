namespace BlazorBLE.Data;

public sealed class RssiLowVarianceSample
{
    private int BeaconCount => beaconGuids.Length;
    
    private readonly Dictionary<Guid, BeaconSamples> samples = new();

    private readonly Guid[] beaconGuids;

    private readonly int minimumSampleCount;

    public RssiLowVarianceSample(Guid[] beaconGuids, int minimumSampleCount = 40)
    {
        this.beaconGuids = beaconGuids;
        this.minimumSampleCount = minimumSampleCount;
        
        foreach (Guid beaconGuid in beaconGuids)
        {
            samples.Add(beaconGuid, new BeaconSamples());
        }
    }

    public void Add(BeaconRssiMeasurement measurement)
    {
        for (int i = 0; i < measurement.BeaconIds.Length; i++)
        {
            Guid beaconGuid = measurement.BeaconIds[i];
            int rssi = measurement.Rssis[i];
            
            samples[beaconGuid].Add(rssi);
        }
    }
    
    public bool IsStable(double threshold)
    {
        if (samples.Count < minimumSampleCount)
        {
            return false;
        }

        int stableCount = 0;
        
        foreach (BeaconSamples beaconSamples in samples.Values)
        {
            if (beaconSamples.IsStable(threshold))
            {
                stableCount++;
            }
        }

        return stableCount == BeaconCount;
    }

    public BeaconRssiMeasurement CalculateAverageMeasurement()
    {
        double[] averages = new double[BeaconCount];
        
        for (int i = 0; i < BeaconCount; i++)
        {
            Guid beaconGuid = beaconGuids[i];
            averages[i] = samples[beaconGuid].Average();
        }
        
        // temporary, I hope hope hope
        int[] roundedAverages = averages.Select(average => (int) Math.Round(average)).ToArray();

        return new BeaconRssiMeasurement(beaconGuids, roundedAverages);
    }
}