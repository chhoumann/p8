using BlazorBLE.Data;
using BlazorBLE.Services;

namespace BlazorBLE.Tests;

public sealed class KnnClassifierTest
{
    [Theory]
    [InlineData(new[] { -80, -70, -60 }, ClassLabel.Inside, 3, 0.5)]
    [InlineData(new[] { -200, 250, 300 }, ClassLabel.Outside, 3, 0.5)]
    public void TestClassifyInside(int[] measurement, ClassLabel expectedLabel, int k, double threshold)
    {
        KnnClassifier classifier = new KnnClassifier(k, threshold);

        IReadOnlyList<DataPoint> rssiDataPoints = new List<DataPoint>
        {
            new DataPoint(ClassLabel.Inside, new double[] { -75, -68, -58 }),
            new DataPoint(ClassLabel.Outside,new double[] { -85, -78, -68 }),
            new DataPoint(ClassLabel.Inside, new double[] { -90, -60, -55 }),
            new DataPoint(ClassLabel.Outside,new double[] { -70, -80, -65 }),
            new DataPoint(ClassLabel.Inside, new double[] { -72, -69, -63 })
        };

        // Act
        ClassLabel classificationLabel = classifier.Classify(measurement, rssiDataPoints);

        // Assert
        Assert.Equal(expectedLabel, classificationLabel);
    }
}