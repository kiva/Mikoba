using System.Collections.Generic;

namespace mikoba.ViewModels.SSI
{
    public class CredentialOverlay
    {
        public string CredentialId;
        public Dictionary<string, CredentialOverlayField> Fields { get; set; }
    }
}