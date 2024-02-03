namespace FantasyLCS.API
{
    public static class SharedLockObjects
    {
        public static readonly object ExternalDataRefreshLock = new object();

        public static readonly object ScoresLock = new object();
    }
}
