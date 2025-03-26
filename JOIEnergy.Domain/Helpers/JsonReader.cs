
using System.Text.Json;

namespace JOIEnergy.Domain.Helpers
{
    public static class JsonReader
    {
        public static T LoadJson<T>(string filePath) where T : class, new()
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return new T();  // Explicitly returning null for reference types
            }

            try
            {
                string jsonContent = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<T>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file {filePath}: {ex.Message}");
                return new T();  // Handle exceptions by returning null
            }
        }
    }
}


