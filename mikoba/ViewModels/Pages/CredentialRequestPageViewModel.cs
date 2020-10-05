using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Input;
using Autofac;
using DynamicData;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Extensions;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using Hyperledger.Indy.AnonCredsApi;
using mikoba.Extensions;
using mikoba.Services;
using Newtonsoft.Json;
using ReactiveUI;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages
{
    public class CredentialRequestPageViewModel : KivaBaseViewModel
    {
        public CredentialRequestPageViewModel(
            INavigationService navigationService,
            IAgentProvider contextProvider,
            IEventAggregator eventAggregator)
            : base("Credential Request", navigationService)
        {
            _contextProvider = contextProvider;
            _eventAggregator = eventAggregator;
        }

        private Transport _transport;
        private RequestPresentationMessage _requestMessage;
        private ProofRequest _proofRequest;
        private MessageContext _requestMessageContext;
        private List<Credential> _potentialCredentials;

        #region Services

        private readonly IAgentProvider _contextProvider;
        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Commands

        public async Task<RequestedCredentials> buildCredentials()
        {
            var _proofService = App.Container.Resolve<IProofService>() as DefaultProofService;
            var _credentialService = App.Container.Resolve<ICredentialService>();
            var requestedCredentials = new RequestedCredentials();
            var context = await _contextProvider.GetContextAsync();

            foreach (var requestedAttribute in _transport.holderProofRequest.RequestedAttributes)
            {
                var credentials =
                    await _proofService.ListCredentialsForProofRequestAsync(context, _transport.holderProofRequest,
                        requestedAttribute.Key);
                
                string credentialId = "";
                if (credentials.Any())
                {
                    credentialId = credentials.First().CredentialInfo.Referent;
                }
                else
                {
                    credentialId = Preferences.Get("credential-id", null);
                }
                
                if (credentialId != null)
                {
                    requestedCredentials.RequestedAttributes.Add(requestedAttribute.Key,
                        new RequestedAttribute
                        {
                            CredentialId = credentialId,
                            Revealed = true
                        });
                }
            }
            return requestedCredentials;
        }

        public ICommand ShareCommand => new Command(async () =>
        {
            var context = await _contextProvider.GetContextAsync();
            var _messageService = App.Container.Resolve<IMessageService>();
            var _proofService = App.Container.Resolve<IProofService>();
            try
            {
                var requestedCredentials = await buildCredentials();
                (var proofMessage, var _) = await _proofService.CreatePresentationAsync(context,
                    _requestMessageContext.ContextRecord.Id, requestedCredentials);
                await _messageService.SendAsync(null, proofMessage, _requestMessageContext.Connection);
                _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.ConnectionsUpdated});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                await NavigationService.PopModalAsync();
            }
        });

        public ICommand DeclineCommand => new Command(async () =>
        {
            try
            {
                var context = await _contextProvider.GetContextAsync();
                var _proofService = App.Container.Resolve<IProofService>() as DefaultProofService;
                await _proofService.RejectProofRequestAsync(context, _requestMessage.Requests[0].Id);
                await NavigationService.PopModalAsync();
            }
            finally
            {
                await NavigationService.PopModalAsync();
            }
        });

        #endregion

        #region UI Properties

        private ImageSource _photoAttach;

        public ImageSource PhotoAttach
        {
            get => _photoAttach;
            set => this.RaiseAndSetIfChanged(ref _photoAttach, value);
        }

        private RangeEnabledObservableCollection<CredentialPreviewAttribute> _requestedAttributes =
            new RangeEnabledObservableCollection<CredentialPreviewAttribute>();

        public RangeEnabledObservableCollection<CredentialPreviewAttribute> RequestedAttributes
        {
            get => _requestedAttributes;
            set => this.RaiseAndSetIfChanged(ref _requestedAttributes, value);
        }

        #endregion

        #region Work

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is Transport transport)
            {
                _transport = transport;
                _requestMessage = transport.presentationMessage;
                _requestMessageContext = transport.messageContext;

                var requestedAttributes = new RangeEnabledObservableCollection<CredentialPreviewAttribute>();
                foreach (var req in _requestMessage.Requests)
                {
                    foreach (var attributes in _transport.holderProofRequest.RequestedAttributes)
                    {
                        var attributeName = attributes.Value.Name;
                        // if (attributeName.Contains("photo~attach")) continue;
                        requestedAttributes.Add(new CredentialPreviewAttribute(attributeName, ""));
                    }
                }

                RequestedAttributes.Clear();
                RequestedAttributes.AddRange(requestedAttributes);
            }

            await base.InitializeAsync(navigationData);
        }

        #endregion
    }
}
