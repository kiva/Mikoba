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
                    try
                    {
                        var _proofService = App.Container.Resolve<IProofService>();
                        var navigation = App.Container.Resolve<INavigationService>();
                        var presentation = messageContext.GetMessage<RequestPresentationMessage>();
                        var holderProofRequest =
                            await _proofService.ProcessRequestAsync(agentContext, presentation, messageContext.Connection);
                        messageContext.ContextRecord = await _proofService.GetAsync(agentContext, holderProofRequest.Id);

                        var transport = new ProofRequestTransport()
                        {
                            Message = presentation,
                            MessageContext = messageContext,
                            Record = messageContext.ContextRecord as ProofRecord
                        };
                        
                        await navigation.NavigateToAsync<ProofRequestViewModel>(transport, NavigationType.Modal);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
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
