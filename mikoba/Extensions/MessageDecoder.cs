using System;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac.Features.GeneratedFactories;
using Hyperledger.Aries;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Extensions;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using Hyperledger.Aries.Routing;
using Hyperledger.Indy.WalletApi;
using Microsoft.AspNetCore.WebUtilities;
using mikoba.UI;

namespace mikoba.Extensions
{
    public static class MessageDecoder
    {
        public static AgentMessage GetInternalMessage(UnpackedMessageContext source)
        {
            switch (source.GetMessageType())
            {
                //MessageTypes.ConnectionInvitation
                case MessageTypes.ConnectionInvitation:
                    return source.GetMessage<ConnectionInvitationMessage>();
                //MessageTypes.PresentProofNames
                case MessageTypes.PresentProofNames.RequestPresentation:
                    return source.GetMessage<RequestPresentationMessage>();
                //MessageTypes.IssueCredentialNames
                case MessageTypes.IssueCredentialNames.OfferCredential:
                    return source.GetMessage<CredentialOfferMessage>();
                default:
                    return null;
            }
        }

        public static async Task<AgentMessage> ProcessPackedMessage(Wallet wallet, InboxItemMessage message,
            string senderKey)
        {
            try
            {
                var bytes = message.Data.GetUTF8Bytes();
                var unpacked = await CryptoUtils.UnpackAsync(wallet, bytes);
                var result = new UnpackedMessageContext(unpacked.Message, senderKey);
                return GetInternalMessage(result);
            }
            catch (Exception e)
            {
                // throw new AriesFrameworkException(ErrorCode.InvalidMessage, "Failed to un-pack message", e);
                return null;
                
            }
        }

        public static async Task<AgentMessage> ParseMessageAsync(string value)
        {
            string messageDecoded = null;
            UnpackedMessageContext unpackedMessage = null;

            try
            {
                if (value.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                    || value.StartsWith("https", StringComparison.OrdinalIgnoreCase)
                    || value.StartsWith("didcomm", StringComparison.OrdinalIgnoreCase))
                {
                    var url = new Uri(value);
                    var query = QueryHelpers.ParseQuery(url.Query);
                    if (query.TryGetValue("c_i", out var messageEncoded) ||
                        query.TryGetValue("d_m", out messageEncoded) ||
                        query.TryGetValue("m", out messageEncoded))
                    {
                        messageDecoded = Uri.UnescapeDataString(messageEncoded);
                    }
                    else
                    {
                        var client = new HttpClient(new HttpClientHandler {AllowAutoRedirect = false});
                        var response = await client.GetAsync(value);
                        var invitationUri = response.Headers.Location;
                        query = QueryHelpers.ParseNullableQuery(invitationUri.Query);
                        if (query.TryGetValue("c_i", out messageEncoded) ||
                            query.TryGetValue("d_m", out messageEncoded) ||
                            query.TryGetValue("m", out messageEncoded))
                        {
                            messageDecoded = Uri.UnescapeDataString(messageEncoded);
                        }
                    }

                    // Because the decoder above strips the +
                    // https://github.com/aspnet/HttpAbstractions/blob/bc7092a32b1943c7f17439e419d3f66cd94ce9bd/src/Microsoft.AspNetCore.WebUtilities/QueryHelpers.cs#L165
                    messageDecoded = messageDecoded.Replace(' ', '+');
                    var json = messageDecoded.GetBytesFromBase64().GetUTF8String();
                    unpackedMessage = new UnpackedMessageContext(json, senderVerkey: null);
                }
                else
                {
                    messageDecoded = Uri.UnescapeDataString(value);
                    var json2 = messageDecoded.GetBytesFromBase64().GetUTF8String();
                    unpackedMessage = new UnpackedMessageContext(json2, senderVerkey: null);
                }

                return GetInternalMessage(unpackedMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error");
                return null;
            }
        }
    }
}
