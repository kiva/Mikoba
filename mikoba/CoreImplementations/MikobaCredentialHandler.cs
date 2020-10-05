using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Hyperledger.Aries;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Extensions;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using Hyperledger.Aries.Storage;
using Microsoft.Extensions.Options;
using mikoba.Services;
using mikoba.ViewModels.Pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mikoba.CoreImplementations
{
    public class MikobaCredentialHandler : IMessageHandler
    {
        private readonly AgentOptions _agentOptions;
        private readonly ICredentialService _credentialService;
        private readonly IWalletRecordService _recordService;

        private static MikobaCredentialHandler Instance { get; set; }


        public MikobaCredentialHandler(
            // ICredentialService credentialService,
            // IWalletRecordService recordService
            )
        {
            
        }

        public IEnumerable<MessageType> SupportedMessageTypes => new MessageType[]
        {
            MessageTypes.IssueCredentialNames.OfferCredential,
            MessageTypes.IssueCredentialNames.RequestCredential,
            MessageTypes.IssueCredentialNames.IssueCredential,
            MessageTypesHttps.IssueCredentialNames.OfferCredential,
            MessageTypesHttps.IssueCredentialNames.RequestCredential,
            MessageTypesHttps.IssueCredentialNames.IssueCredential
        };


        public async Task<AgentMessage> ProcessAsync(IAgentContext agentContext, UnpackedMessageContext messageContext)
        {
            switch (messageContext.GetMessageType())
            {
                // v1
                case MessageTypesHttps.IssueCredentialNames.OfferCredential:
                case MessageTypes.IssueCredentialNames.OfferCredential:
                {
                    var _proofService = App.Container.Resolve<IProofService>();
                    var navigation = App.Container.Resolve<INavigationService>();
                    
                    var offer = messageContext.GetMessage<CredentialOfferMessage>();
                    var recordId = await _credentialService.ProcessOfferAsync(
                        agentContext, offer, messageContext.Connection);

                    messageContext.ContextRecord = await _credentialService.GetAsync(agentContext, recordId);

                    // Auto request credential if set in the agent option
                    if (_agentOptions.AutoRespondCredentialOffer)
                    {
                        
                    }
                    
                    await navigation.NavigateToAsync<CredentialRequestPageViewModel>(message,NavigationType.Modal);

                    return null;
                }
                case MessageTypesHttps.IssueCredentialNames.RequestCredential:
                case MessageTypes.IssueCredentialNames.RequestCredential:
                {
                    var request = messageContext.GetMessage<CredentialRequestMessage>();
                    var recordId = await _credentialService.ProcessCredentialRequestAsync(
                        agentContext: agentContext,
                        credentialRequest: request,
                        connection: messageContext.Connection);
                    if (request.ReturnRoutingRequested() && messageContext.Connection == null)
                    {
                        var (message, record) = await _credentialService.CreateCredentialAsync(agentContext, recordId);
                        messageContext.ContextRecord = record;
                        return message;
                    }
                    else
                    {
                        // Auto create credential if set in the agent option
                        if (_agentOptions.AutoRespondCredentialRequest)
                        {
                            var (message, record) =
                                await _credentialService.CreateCredentialAsync(agentContext, recordId);
                            messageContext.ContextRecord = record;
                            return message;
                        }

                        messageContext.ContextRecord = await _credentialService.GetAsync(agentContext, recordId);
                        return null;
                    }
                }
                case MessageTypesHttps.IssueCredentialNames.IssueCredential:
                case MessageTypes.IssueCredentialNames.IssueCredential:
                {
                    var credential = messageContext.GetMessage<CredentialIssueMessage>();
                    var recordId = await _credentialService.ProcessCredentialAsync(
                        agentContext, credential, messageContext.Connection);

                    messageContext.ContextRecord = await UpdateValuesAsync(
                        credentialId: recordId,
                        credentialIssue: messageContext.GetMessage<CredentialIssueMessage>(),
                        agentContext: agentContext);

                    return null;
                }
                default:
                    throw new AriesFrameworkException(ErrorCode.InvalidMessage,
                        $"Unsupported message type {messageContext.GetMessageType()}");
            }
        }

        private async Task<CredentialRecord> UpdateValuesAsync(string credentialId,
            CredentialIssueMessage credentialIssue, IAgentContext agentContext)
        {
            var credentialAttachment = credentialIssue.Credentials.FirstOrDefault(x => x.Id == "libindy-cred-0")
                                       ?? throw new ArgumentException("Credential attachment not found");

            var credentialJson = credentialAttachment.Data.Base64.GetBytesFromBase64().GetUTF8String();

            var jcred = JObject.Parse(credentialJson);
            var values = jcred["values"].ToObject<Dictionary<string, AttributeValue>>();

            var credential = await _credentialService.GetAsync(agentContext, credentialId);
            credential.CredentialAttributesValues = values.Select(x => new CredentialPreviewAttribute
                {Name = x.Key, Value = x.Value.Raw, MimeType = CredentialMimeTypes.TextMimeType}).ToList();
            await _recordService.UpdateAsync(agentContext.Wallet, credential);

            return credential;
        }

        private class AttributeValue
        {
            [JsonProperty("raw")] public string Raw { get; set; }

            [JsonProperty("encoded")] public string Encoded { get; set; }
        }
    }
}
