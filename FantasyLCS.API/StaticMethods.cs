public static class StaticMethods
{
    public static int CreateUniqueIdFromString(string teamName)
    {
        int hash = teamName.GetHashCode();
        int uniqueId = Math.Abs(hash) % 90000 + 10000; // Transform hash into a 5-digit number
        return uniqueId;
    }
}
