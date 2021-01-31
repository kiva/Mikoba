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
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Options;
using mikoba.Services;
using mikoba.ViewModels.Pages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sentry;

namespace mikoba.CoreImplementations
{
    public class MikobaCredentialHandler : IMessageHandler
    {
//TODO: Fix unused dependencies        
#pragma warning disable 649
        private readonly AgentOptions _agentOptions;
        private readonly ICredentialService _credentialService;
        private readonly IWalletRecordService _recordService;
#pragma warning restore 649

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

        /*
         
         [Issuer: Creates offer for Holder.]
         [Issuer: Sends offer for Holder.]
         
         Holder: Receives credential offer.
         Holder: Process the credential offer by storing it.
         
         (Holder: Creates master secret if not already.)
         
         Holder: Sends a credential request using the stored offer id.
         
         [Issuer: Retrieved credential request]
         [Issuer: Receives the credential request and sends a credential issue message]
         
         Holder: Receives the credential issue message.
         Holder: Process the message and store its.
         */
        

        public async Task<AgentMessage> ProcessAsync(IAgentContext agentContext, UnpackedMessageContext messageContext)
        {
            switch (messageContext.GetMessageType())
            {
                // v1
                case MessageTypesHttps.IssueCredentialNames.OfferCredential:
                case MessageTypes.IssueCredentialNames.OfferCredential:
                {
                    try
                    {
                        var navigation = App.Container.Resolve<INavigationService>();
                        var _credentialService = App.Container.Resolve<ICredentialService>();

                        var offer = messageContext.GetMessage<CredentialOfferMessage>();

                        Console.WriteLine("New Credential Offering: {0}", offer.Id);

                        var recordId = await _credentialService.ProcessOfferAsync(
                            agentContext, offer, messageContext.Connection);

                        messageContext.ContextRecord = await _credentialService.GetAsync(agentContext, recordId);

                        var transport = new CredentialOfferTransport()
                        {
                            Record = messageContext.ContextRecord as CredentialRecord,
                            Message = offer,
                            MessageContext = messageContext
                        };

                        await navigation.NavigateToAsync<CredentialOfferPageViewModel>(transport, NavigationType.Modal);
                    }
                    catch (Exception ex)
                    {
                        Tracking.TrackException(ex);
                    }
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
                        if (_agentOptions != null && _agentOptions.AutoRespondCredentialRequest)
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
                    try
                    {
                        var navigation = App.Container.Resolve<INavigationService>();
                        var _credentialService = App.Container.Resolve<ICredentialService>();

                        var credential = messageContext.GetMessage<CredentialIssueMessage>();
                        var recordId = await _credentialService.ProcessCredentialAsync(
                            agentContext, credential, messageContext.Connection);

                        messageContext.ContextRecord = await UpdateValuesAsync(
                            credentialId: recordId,
                            credentialIssue: messageContext.GetMessage<CredentialIssueMessage>(),
                            agentContext: agentContext);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                        SentrySdk.CaptureException(ex);
                        Console.WriteLine(ex.Message);
                    }
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
