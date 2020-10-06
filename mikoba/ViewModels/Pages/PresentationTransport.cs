using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Features.PresentProof;

namespace mikoba.ViewModels.Pages
{
    public class ProofRequestTransport
    {
        public ProofRecord holderProofRecord { get; set; }
        public RequestPresentationMessage presentationMessage { get; set; }
        public MessageContext messageContext { get; set; }
        public ProofRequest holderProofRequest { get; set; }
    }
}
