namespace mikoba.Extensions
{
    public enum DispatchType
    {
        ConnectionsUpdated,
        ConnectionCreated,
        NotificationDismissed,
    }

    public class CoreDispatchedEvent
    {
        public DispatchType Type { get; set; }
    }
}
