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
        CredentialRemoved,
        CredentialShared,
        CredentialShareFailed
    }

    public class CoreDispatchedEvent
    {
        public DispatchType Type { get; set; }
    }
}
