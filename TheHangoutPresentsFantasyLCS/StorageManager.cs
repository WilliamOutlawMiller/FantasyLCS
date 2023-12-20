using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class StorageManager
{
    public List<T> ReadData<T>() where T : class
    {
        List<T> dataList = new List<T>();
        string filePath = $"{typeof(T).Name.ToLower()}.json";

        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                dataList = JsonSerializer.Deserialize<List<T>>(json);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading data: {ex.Message}");
        }

        return dataList;
    }

    public void WriteData<T>(List<T> dataList) where T : class
    {
        string filePath = $"{typeof(T).Name.ToLower()}.json";

        try
        {
            string json = JsonSerializer.Serialize(dataList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing data: {ex.Message}");
        }
    }

    public static void UpdateStorageFiles<T>(List<T> newDataList) where T : class, new()
    {
        StorageManager storageManager = new StorageManager();
        var existingData = storageManager.ReadData<T>();

        foreach (var newData in newDataList)
        {
            var existingItem = existingData.FirstOrDefault(item => AreEqual(item, newData));

            if (existingItem != null)
            {
                UpdateItemProperties(existingItem, newData);
            }
            else
            {
                existingData.Add(newData);
            }
        }

        storageManager.WriteData(existingData);
    }

    private static bool AreEqual<T>(T obj1, T obj2)
    {
        // Create a dictionary to store property names and values
        Dictionary<string, object> obj1Properties = obj1.GetType().GetProperties()
            .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj1));

        Dictionary<string, object> obj2Properties = obj2.GetType().GetProperties()
            .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj2));

        // Compare the dictionaries
        return obj1Properties.Count == obj2Properties.Count &&
               obj1Properties.All(kv => obj2Properties.TryGetValue(kv.Key, out var value) && Equals(kv.Value, value));
    }

    private static void UpdateItemProperties<T>(T existingItem, T newItem)
    {
        var props = typeof(T).GetProperties();
        foreach (var prop in props)
        {
            if (prop.CanWrite && prop.Name != "Name") // Assuming 'Name' should not be updated
            {
                var newValue = prop.GetValue(newItem);
                prop.SetValue(existingItem, newValue);
            }
        }
    }
}
