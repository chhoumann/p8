using BlazorBLE.Data;

namespace BlazorBLE.Services
{
    public sealed class RssiDataHandler
    {
        public int[] Averages { get; private set; }

        private readonly ShiftList<int[]> latestMeasurements = new(10);
        
        private RssiDataSet dataSet;

        public void LoadData(string jsonFileName)
        {
            dataSet = RssiDataSet.ReadFromJson(jsonFileName);
            dataSet.RemoveDuplicates();
        }
        
        public bool IsInsideRoom(int[] measurement, double distanceThreshold)
        {
            if (measurement.Length != dataSet.NumBeacons)
            {
                return false;
            }

            bool isInsideRoom = false;

            latestMeasurements.Add(measurement);
            Averages = CalculateAverages(latestMeasurements);
            
            foreach (int[] distance in dataSet.RssiDataPoints)
            {
                double dist = GetDistance(distance, Averages);

                if (dist < distanceThreshold)
                {
                    isInsideRoom = true;
                }
            }

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

        private static int[] CalculateAverages(IList<int[]> list)
        {
            int[] averages = new int[list[0].Length];
            
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Length; j++)
                {
                    averages[j] += list[i][j];
                }
            }
            
            for (int i = 0; i < averages.Length; i++)
            {
                averages[i] /= list.Count;
            }
            
            return averages;
        }
    }
}