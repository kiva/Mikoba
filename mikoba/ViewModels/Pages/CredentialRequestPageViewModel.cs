using System;
using System.Collections.Generic;
using System.IO;
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
using mikoba.ViewModels.SSI;
using Newtonsoft.Json;
using ReactiveUI;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages
{
    public class CredentialRequestPageViewModel : KivaBaseViewModel
    {
        private static readonly string[] AllowedFields = {"nationalId", "photo~attach", "dateOfBirth", "birthDate","firstName", "lastName"};
        
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
        private CredentialRecord _credential; 

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
                    credentialId = _credential.Id;
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
                this.ShowReceipt = true;
            }
        });
        
        public ICommand CloseReceiptCommand => new Command(async () =>
        {
            await NavigationService.PopModalAsync();
        });

        public ICommand DeclineCommand => new Command(async () =>
        {
            try
            {
                var context = await _contextProvider.GetContextAsync();
                var _proofService = App.Container.Resolve<IProofService>() as DefaultProofService;
                await _proofService.RejectProofRequestAsync(context, _requestMessage.Requests[0].Id);
            }
            finally
            {
                await NavigationService.PopModalAsync();
            }
        });

        #endregion

        #region UI Properties

        
        private bool _showReceipt;

        public bool ShowReceipt
        {
            get => _showReceipt;
            set => this.RaiseAndSetIfChanged(ref _showReceipt, value);
        }
        
        private ImageSource _photoAttach;

        public ImageSource PhotoAttach
        {
            get => _photoAttach;
            set => this.RaiseAndSetIfChanged(ref _photoAttach, value);
        }

        private RangeEnabledObservableCollection<SSICredentialAttribute> _requestedAttributes =
            new RangeEnabledObservableCollection<SSICredentialAttribute>();

        public RangeEnabledObservableCollection<SSICredentialAttribute> RequestedAttributes
        {
            get => _requestedAttributes;
            set => this.RaiseAndSetIfChanged(ref _requestedAttributes, value);
        }

        #endregion

        #region Work

        public override async Task InitializeAsync(object navigationData)
        {
            var context = await _contextProvider.GetContextAsync();
            if (navigationData is Transport transport)
            {
                _transport = transport;
                _requestMessage = transport.presentationMessage;
                _requestMessageContext = transport.messageContext;

                var requestedAttributes = new RangeEnabledObservableCollection<SSICredentialAttribute>();
                var _credentialService = App.Container.Resolve<ICredentialService>();
                var _scope = App.Container.Resolve<ILifetimeScope>();
                
                var credentialsRecords = await _credentialService.ListAsync(context);
                _credential = credentialsRecords.First();
                var credentialViewModel = _scope.Resolve<SSICredentialViewModel>(new NamedParameter("credential", _credential));
                foreach (var attribute in credentialViewModel.Attributes)
                {
                    if (!AllowedFields.Contains(attribute.Name)) continue;
                    if (attribute.Name.Contains("~") && PhotoAttach == null)
                    {
                        PhotoAttach = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(attribute.Value.ToString())));    
                    }
                    else
                    {
                        requestedAttributes.Add(new SSICredentialAttribute()
                        {
                            Name = attribute.Name,
                            Value = attribute.Value,
                        });
                    }
                }
                
                
                // foreach (var req in _requestMessage.Requests)
                // {
                //     foreach (var attributes in _transport.holderProofRequest.RequestedAttributes)
                //     {
                //         var attributeName = attributes.Value.Name;
                //         //if (attributeName.Contains("photo~attach")) continue;
                //         requestedAttributes.Add(new SSICredentialAttribute()
                //         {
                //             Name = attributeName
                //         });
                //     }
                // }
                
                
                RequestedAttributes.Clear();
                RequestedAttributes.AddRange(requestedAttributes);
            }
            else if (navigationData is SSICredentialViewModel credential)
            {
                var requestedAttributes = new List<SSICredentialAttribute>();
                foreach (var attribute in credential.Attributes)
                {
                    if (!AllowedFields.Contains(attribute.Name)) continue;
                    if (attribute.Name.Contains("~") && PhotoAttach == null)
                    {
                        PhotoAttach = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(attribute.Value.ToString())));    
                    }
                    else
                    {
                        requestedAttributes.Add(new SSICredentialAttribute()
                        {
                            Name = attribute.Name,
                            Value = attribute.Value,
                        });
                    }
                }
                RequestedAttributes = new RangeEnabledObservableCollection<SSICredentialAttribute>();
                RequestedAttributes.AddRange(requestedAttributes);
            }
            await base.InitializeAsync(navigationData);
        }

        #endregion
    }
}
