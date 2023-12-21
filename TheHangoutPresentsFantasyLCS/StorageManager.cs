using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class StorageManager
{   
    public static void UpdateStorageFiles<T>(List<T> newDataList) where T : class, new()
    {
        StorageManager storageManager = new StorageManager();
        var existingData = storageManager.ReadData<T>();

        foreach (var newData in newDataList)
        {
            var newDataId = typeof(T).GetProperty("ID").GetValue(newData);

            var existingItem = existingData.FirstOrDefault(item => 
                typeof(T).GetProperty("ID").GetValue(item).Equals(newDataId));

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

    public static void UpdateStorageFiles<T>(T newData) where T : class, new()
    {
        UpdateStorageFiles(new List<T> { newData });
    }
    
    private static void UpdateItemProperties<T>(T existingItem, T newItem)
    {
        var props = typeof(T).GetProperties();
        foreach (var prop in props)
        {
            if (prop.CanWrite && prop.Name != "ID") // Assuming 'ID' should not be updated
            {
                var newValue = prop.GetValue(newItem);
                prop.SetValue(existingItem, newValue);
            }
        }
    }

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
}