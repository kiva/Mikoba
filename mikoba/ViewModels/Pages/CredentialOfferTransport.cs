using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;

namespace mikoba.ViewModels.Pages
{
    public class CredentialOfferTransport
    {
        public CredentialRecord Record { get; set; }
        public CredentialOfferMessage Message { get; set; }
        public MessageContext MessageContext { get; set; }
    }
}
