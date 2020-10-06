using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Features.PresentProof;

namespace mikoba.ViewModels.Pages
{
    public class ProofRequestTransport
    {
        public ProofRecord Record { get; set; }
        public RequestPresentationMessage Message { get; set; }
        public MessageContext MessageContext { get; set; }
    }
}
