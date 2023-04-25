using BlazorBLE.Data;

namespace BlazorBLE.Services
{
    public sealed class RssiDataHandler
    {
        public double[] Averages { get; private set; }

        private readonly ShiftList<double[]> latestMeasurements = new(10);
        
        private RssiDataSet dataSet;

        private List<double[]> measurementsInMeters = new();

        private bool isConverted;

        public void LoadData(string jsonFileName)
        {
            dataSet = RssiDataSet.ReadFromJson(jsonFileName);
            dataSet.RemoveDuplicates();
        }
        
        public bool IsInsideRoom(int[] measurement, int txPower, double environmentalFactor, double distanceThreshold)
        {
            if (!isConverted)
            {
                measurementsInMeters = dataSet.ToMeters(txPower, environmentalFactor);
                isConverted = true;
            }
            
            if (measurement.Length != dataSet.NumBeacons)
            {
                return false;
            }

            bool isInsideRoom = false;

            double lowestDistance = double.MaxValue;
            double[] measurementInMeters = BLEMath.ToMeters(measurement, txPower, environmentalFactor);

            latestMeasurements.Add(measurementInMeters);
            
            Averages = CalculateAverages(latestMeasurements);
            
            foreach (double[] distance in measurementsInMeters)
            {
                double dist = GetDistance(distance, Averages);

                if (dist < lowestDistance)
                {
                    lowestDistance = dist;
                }

                if (dist < distanceThreshold)
                {
                    isInsideRoom = true;
                }
            }

            Console.WriteLine($"Lowest distance = {lowestDistance} meters");
            Console.WriteLine($"Inside room = {isInsideRoom}");
            
            // write the averages array to the console
            Console.Write("Averages:");
            foreach (double average in Averages)
            {
                Console.Write($"{average} ");
            }
            Console.WriteLine();
            

            return isInsideRoom;
        }

        private static double GetDistance(IReadOnlyList<double> vector1, IReadOnlyList<double> vector2)
        {
            double distanceSquared = 0;

            for (int i = 0; i < vector1.Count; i++)
            {
                double difference = vector1[i] - vector2[i];
                distanceSquared += difference * difference;
            }

            return Math.Sqrt(distanceSquared);
        }

        private static double[] CalculateAverages(ShiftList<double[]> list)
        {
            double[] averages = new double[list.List[0].Length];
            
            for (int i = 0; i < list.List.Count; i++)
            {
                for (int j = 0; j < list.List[i].Length; j++)
                {
                    averages[j] += list.List[i][j];
                }
            }
            
            for (int i = 0; i < averages.Length; i++)
            {
                averages[i] /= list.List.Count;
            }
            
            return averages;
        }
    }
}