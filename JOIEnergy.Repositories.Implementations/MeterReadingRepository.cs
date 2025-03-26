using System.Text.Json;
using JOIEnergy.Domain.Helpers;
using JOIEnergy.Domain.Models;
using JOIEnergy.Repository.Interfaces;
using Microsoft.Extensions.Configuration;

namespace JOIEnergy.Repository.Implementations
{

    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly string _jsonFilePath;
        private Dictionary<string, List<ElectricityReading>> _readings;

        public MeterReadingRepository(IConfiguration configuration)
        {
            _jsonFilePath = GetJsonFilePath(configuration);
            InitializeReadings(configuration);
        }

        private string GetJsonFilePath(IConfiguration configuration)
        {
            string filePath = configuration.GetValue<string>("MeterReadingSettings:JsonFilePath") ?? "smartMeterReadings.json";

            // Combine with base directory to locate the file dynamically
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", filePath);
        }

        private void InitializeReadings(IConfiguration configuration)
        {
            bool shouldGenerateReadings = configuration.GetValue<bool>("MeterReadingSettings:GenerateReadingsOnStartup");
            int daysToGenerate = configuration.GetValue<int>("MeterReadingSettings:DaysToGenerate");
            int readingsPerDay = configuration.GetValue<int>("MeterReadingSettings:ReadingsPerDay");

            // Ensure directory exists before writing JSON
            Directory.CreateDirectory(Path.GetDirectoryName(_jsonFilePath));

            if (shouldGenerateReadings)
            {

                var generatedReadings = ElectricityReadingGenerator.GenerateReadings(daysToGenerate, readingsPerDay);
                _readings = generatedReadings.ToDictionary(m => m.SmartMeterId, m => m.ElectricityReadings);
                SaveReadingsToJson();
            }
            else if (File.Exists(_jsonFilePath))
            {

                var readingsList = JsonReader.LoadJson<List<MeterReadings>>(_jsonFilePath) ?? new List<MeterReadings>();
                _readings = readingsList.ToDictionary(m => m.SmartMeterId, m => m.ElectricityReadings);
            }
            else
            {
                _readings = new Dictionary<string, List<ElectricityReading>>();
            }
        }


        public List<ElectricityReading> GetReadings(string smartMeterId)
        {
            return _readings.ContainsKey(smartMeterId) ? _readings[smartMeterId] : new List<ElectricityReading>();
        }

        public void StoreReadings(string smartMeterId, List<ElectricityReading> readings)
        {
            if (string.IsNullOrWhiteSpace(smartMeterId) || readings == null || !readings.Any())
                throw new ArgumentException("Invalid smart meter ID or readings.");

            if (!_readings.ContainsKey(smartMeterId))
                _readings[smartMeterId] = new List<ElectricityReading>();

            _readings[smartMeterId].AddRange(readings);
            SaveReadingsToJson();
        }

        private void SaveReadingsToJson()
        {
            try
            {
                string json = JsonSerializer.Serialize(
                    _readings.Select(kvp => new MeterReadings
                    {
                        SmartMeterId = kvp.Key,
                        ElectricityReadings = kvp.Value
                    }).ToList(),
                    new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(_jsonFilePath, json);
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Error saving smart meter readings to {_jsonFilePath}");
            }
        }
    }

}
