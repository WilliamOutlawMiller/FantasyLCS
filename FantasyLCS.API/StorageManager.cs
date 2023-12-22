using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

    public static void UpdateData<T>(T data) where T : class, new()
    {
        var dataList = ReadData<T>();
        var idProperty = data.GetType().GetProperty("ID");

        var dataId = (int)idProperty.GetValue(data);
        var itemIndex = dataList.FindIndex(i => (int)i.GetType().GetProperty("ID").GetValue(i) == dataId);

        if (itemIndex != -1)
        {
            UpdateProperties(dataList[itemIndex], data);
        }
        else
        {
            dataList.Add(data);
        }

        WriteData(dataList);
    }

    public static void UpdateData<T>(List<T> updatedDataList) where T : class, new()
    {
        var dataList = ReadData<T>();

        foreach (var data in updatedDataList)
        {
            var idProperty = data.GetType().GetProperty("ID");

            var dataId = (int)idProperty.GetValue(data);
            var itemIndex = dataList.FindIndex(i => (int)i.GetType().GetProperty("ID").GetValue(i) == dataId);

            if (itemIndex != -1)
            {
                UpdateProperties(dataList[itemIndex], data);
            }
            else
            {
                dataList.Add(data);
            }
        }

        WriteData(dataList);
    }

    private static void UpdateProperties<T>(T existingItem, T newItem) where T : class
    {
        PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (PropertyInfo property in properties)
        {
            var existingValue = property.GetValue(existingItem);
            var newValue = property.GetValue(newItem);

            if ((existingValue != null && !existingValue.Equals(newValue)) || (existingValue == null && newValue != null))
            {
                property.SetValue(existingItem, newValue);
            }
        }
    }
        
    public static T Get<T>(int id) where T : class
    {
        List<T> data = ReadData<T>();

        return data.FirstOrDefault(item => 
        {
            var idProperty = item.GetType().GetProperty("ID");
            if (idProperty != null)
            {
                var value = idProperty.GetValue(item);
                return value != null && (int)value == id;
            }
            return false;
        });
    }

    public static List<T> Get<T>() where T : class
    {
        return ReadData<T>();
    }

    public static void Add<T>(T data) where T : class, new()
    {
        var dataList = ReadData<T>();

        // Add the new data to the end of the list
        dataList.Add(data);

        // Write the updated list back to the file
        WriteData(dataList);
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