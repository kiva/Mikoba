namespace mikoba.Extensions
{
    public enum DispatchType
    {
        ConnectionsUpdated,
        ConnectionCreated,
        NotificationDismissed,
        CredentialAccepted,
        CredentialDeclined,
        CredentialAcceptanceFailed,
        CredentialRemoved
    }

    public class CoreDispatchedEvent
    {
        public DispatchType Type { get; set; }
    }
}
