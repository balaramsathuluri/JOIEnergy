using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JOIEnergy.Repository.Implementations.Utilities
{
    public static class JsonReader
    {
        public static T LoadJson<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return default;
            }

            try
            {
                string jsonContent = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<T>(jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file {filePath}: {ex.Message}");
                return default;
            }
        }
    }
}
