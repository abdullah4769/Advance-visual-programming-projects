using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace InventoryAndSalesManagementSystem.Services
{
    public class FileStorageService<T>
    {
        private readonly string _filePath;

        public FileStorageService(string fileName)
        {
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            _filePath = Path.Combine(directory, fileName);
        }

        public List<T> LoadData()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading data from {_filePath}: {ex.Message}");
                return new List<T>();
            }
        }

        public void SaveData(List<T> data)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(data, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing data to {_filePath}: {ex.Message}");
            }
        }
    }
}
