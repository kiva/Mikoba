using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Hyperledger.Aries;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Features.PresentProof;
using mikoba.Services;
using mikoba.ViewModels.Pages;
using Newtonsoft.Json;

namespace mikoba.CoreImplementations
{
    public class MikobaProofHandler : IMessageHandler
    {
        public MikobaProofHandler()
        {
            
        }


        public IEnumerable<MessageType> SupportedMessageTypes => new MessageType[]
        {
            MessageTypes.PresentProofNames.ProposePresentation,
            MessageTypes.PresentProofNames.Presentation,
            MessageTypes.PresentProofNames.RequestPresentation,
            MessageTypesHttps.PresentProofNames.ProposePresentation,
            MessageTypesHttps.PresentProofNames.Presentation,
            MessageTypesHttps.PresentProofNames.RequestPresentation
        };

        public async Task<AgentMessage> ProcessAsync(IAgentContext agentContext, UnpackedMessageContext messageContext)
        {
            switch (messageContext.GetMessageType())
            {
                // v1.0
                case MessageTypesHttps.PresentProofNames.ProposePresentation:
                case MessageTypes.PresentProofNames.ProposePresentation:
                {
                    var _proofService = App.Container.Resolve<IProofService>();
                    var message = messageContext.GetMessage<ProposePresentationMessage>();
                    var record = await _proofService.ProcessProposalAsync(agentContext, message, messageContext.Connection);
                    messageContext.ContextRecord = record;
                    break;
                }
                case MessageTypes.PresentProofNames.RequestPresentation:
                case MessageTypesHttps.PresentProofNames.RequestPresentation:
                {
                    var _proofService = App.Container.Resolve<IProofService>();
                    
                    var navigation = App.Container.Resolve<INavigationService>();
                    var message = messageContext.GetMessage<RequestPresentationMessage>();
                    
                    var holderProofRequestId = await _proofService.ProcessRequestAsync(agentContext, message, messageContext.Connection);
                    var holderProofRecord = await _proofService.GetAsync(agentContext, holderProofRequestId.Id);
                    
                    messageContext.ContextRecord = holderProofRequestId;
                    
                    var transport = new Transport();
                    transport.presentationMessage = message;
                    transport.messageContext = messageContext;
                    transport.holderProofRecord = holderProofRecord;
                    transport.holderProofRequest = JsonConvert.DeserializeObject<ProofRequest>(holderProofRecord.RequestJson);
                    
                    await navigation.NavigateToAsync<CredentialRequestPageViewModel>(transport,NavigationType.Modal);
                    break;
                }
                case MessageTypes.PresentProofNames.Presentation:
                case MessageTypesHttps.PresentProofNames.Presentation:
                {
                    var _proofService = App.Container.Resolve<IProofService>();
                    var message = messageContext.GetMessage<PresentationMessage>();
                    var record = await _proofService.ProcessPresentationAsync(agentContext, message);
                    messageContext.ContextRecord = record;
                    break;
                }
                default:
                    throw new AriesFrameworkException(ErrorCode.InvalidMessage,
                        $"Unsupported message type {messageContext.GetMessageType()}");
            }
            return null;
        }
    }
}
