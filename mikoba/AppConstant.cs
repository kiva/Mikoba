namespace mikoba
{
    public class AppConstant
    {
        public const string IosAnalyticsKey = "injected-via-ci";

        public const string AndroidAnalyticsKey = "injected-via-ci";

        internal const string LocalWalletProvisioned = "aries.settings.walletprovisioned-1";
        internal const string LocalWalletFirstView = "aries.settings.walletfirstview";
        internal const string EnableFirstActionsView = "mikoba.EnableFirstActionsView";
        internal const string FullName = "mikoba.fullName";
        internal const string PIN = "mikoba.PIN";
        internal const string AllowFingerprint = "mikoba.AllowFingerprint";
        
        internal const string EndpointUri = "https://ec2.protocol-dev.kiva.org/";
        public static string DefaultMasterSecret = "TheMikobaMasterSecret";
    }
}
