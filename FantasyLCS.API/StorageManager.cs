using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class StorageManager
{   
    public static List<T> ReadData<T>() where T : class
    {
        List<T> dataList = new List<T>();
        string filePath = $"JsonStorage/{typeof(T).Name.ToLower()}.json";

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

    public static void WriteData<T>(List<T> dataList) where T : class
    {
        string filePath = $"JsonStorage/{typeof(T).Name.ToLower()}.json";

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

    public static bool ShouldRefreshData<T>() where T : class
    {
        string filePath = $"JsonStorage/{typeof(T).Name.ToLower()}.json";
        
        if (File.Exists(filePath))
        {
            var lastWriteTime = File.GetLastWriteTime(filePath);
            return DateTime.Now - lastWriteTime >= TimeSpan.FromDays(7); // Refresh if older than a week
        }
        else
        {
            return true; // Refresh if file does not exist
        }
    }
}