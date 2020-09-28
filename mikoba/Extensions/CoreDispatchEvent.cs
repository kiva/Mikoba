namespace mikoba.Extensions
{
    public enum DispatchType
    {
        ConnectionsUpdated,
        ConnectionCreated,
    }

    public class CoreDispatchedEvent
    {
        public DispatchType Type { get; set; }
    }
}
