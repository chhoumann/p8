using BlazorBLE.Data;
using Microsoft.Maui.Layouts;

namespace BlazorBLE.Services
{
    public sealed class RssiDataHandler
    {
        private RssiDataMeasurements measurements;

        public void LoadData(string jsonFileName)
        {
            measurements = RssiDataMeasurements.ReadFromJson(jsonFileName);
            measurements.RemoveDuplicates();
        }

        public bool IsInsideRoom(int[] measurement)
        {
            if (measurement.Length != measurements.NumBeacons)
            {
                return false;
            }

            foreach (int[] dataPoints in measurements.RssiDataPoints)
            {
                bool[] inside = new bool[measurements.NumBeacons];

                for (int i = 0; i < measurements.NumBeacons; i++)
                {
                    inside[i] = measurement[i] == dataPoints[i];
                }
                
                if (inside.All(val => val))
                {
                    Console.WriteLine("Inside room = true");
                    return true;
                }
            }

            Console.WriteLine("Inside room = false");
            return false;
        }
    }
}