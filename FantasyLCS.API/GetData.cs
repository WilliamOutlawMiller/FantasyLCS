using static StorageManager;

public class GetData
{
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
}
