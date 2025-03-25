
using JOIEnergy.Domain.Models;

namespace JOIEnergy.Domain.Helpers
{
    public class ElectricityReadingGenerator
    {
        private static readonly Random _random = new Random();

        public static List<MeterReadings> GenerateReadings(int days, int readingsPerDay)
        {
            List<MeterReadings> smartMeterReadingsList = new();

            string[] smartMeterIds = { "smart-meter-0", "smart-meter-1", "smart-meter-2", "smart-meter-3", "smart-meter-4" };
            Random _random = new Random();

            foreach (var meterId in smartMeterIds)
            {
                List<ElectricityReading> readings = new();
                DateTime startDate = DateTime.UtcNow.AddDays(-days);

                for (int i = 0; i < days; i++)
                {
                    for (int j = 0; j < readingsPerDay; j++)
                    {
                        readings.Add(new ElectricityReading
                        {
                            Time = startDate.AddDays(i).AddHours(_random.Next(0, 24)).AddMinutes(_random.Next(0, 60)),
                            Reading = Math.Round((decimal)(_random.NextDouble() * 30), 2) // ✅ Explicit decimal conversion
                        });
                    }
                }

                smartMeterReadingsList.Add(new MeterReadings
                {
                    SmartMeterId = meterId,
                    ElectricityReadings = readings
                });
            }

            return smartMeterReadingsList;
        }

    }
}
