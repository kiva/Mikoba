using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using DynamicData;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using mikoba.Extensions;
using mikoba.Services;
using mikoba.ViewModels.SSI;
using mikoba.UI.Helpers;
using Newtonsoft.Json;
using ReactiveUI;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages
{
    public class ProofRequestViewModel : KivaBaseViewModel
    {
        private static readonly string[] AllowedFields =
            {"nationalId", "photo~attach", "dateOfBirth", "birthDate", "firstName", "lastName"};

        public ProofRequestViewModel(
            INavigationService navigationService,
            IAgentProvider contextProvider,
            IEventAggregator eventAggregator)
            : base("Proof Request", navigationService)
        {
            _contextProvider = contextProvider;
            _eventAggregator = eventAggregator;
        }

        private ProofRequestTransport _proofRequestTransport;
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

            var proofRequest = JsonConvert.DeserializeObject<ProofRequest>(_proofRequestTransport.Record.RequestJson);

            foreach (var requestedAttribute in proofRequest.RequestedAttributes)
            {
                var credentials =
                    await _proofService.ListCredentialsForProofRequestAsync(context, proofRequest,
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
                    _proofRequestTransport.Record.Id, requestedCredentials);
                await _messageService.SendAsync(context.Wallet, proofMessage,
                    _proofRequestTransport.MessageContext.Connection);
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

        public ICommand CloseReceiptCommand => new Command(async () => { await NavigationService.PopModalAsync(); });

        public ICommand DeclineCommand => new Command(async () =>
        {
            try
            {
                var context = await _contextProvider.GetContextAsync();
                var _proofService = App.Container.Resolve<IProofService>() as DefaultProofService;
                await _proofService.RejectProofRequestAsync(context, _proofRequestTransport.Message.Requests[0].Id);
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
            if (navigationData is ProofRequestTransport transport)
            {
                var requestedAttributes = new RangeEnabledObservableCollection<SSICredentialAttribute>();
                var _credentialService = App.Container.Resolve<ICredentialService>();
                var _scope = App.Container.Resolve<ILifetimeScope>();

                _proofRequestTransport = transport;

                var credentialsRecords = await _credentialService.ListAsync(context);
                _credential = credentialsRecords.First();
                var credentialViewModel =
                    _scope.Resolve<SSICredentialViewModel>(new NamedParameter("credential", _credential));
                foreach (var attribute in credentialViewModel.Attributes)
                {
                    // TODO: "No image found" placeholder
                    if (attribute.Name.Contains("~") && PhotoAttach != null)
                    {
                        string value = PhotoAttachParser.ReturnAttachment(attribute.Value.ToString());
                        PhotoAttach = ImageSource.FromStream(() =>
                            new MemoryStream(Convert.FromBase64String(value)));
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

                RequestedAttributes.Clear();
                RequestedAttributes.AddRange(requestedAttributes);
            }

            await base.InitializeAsync(navigationData);
        }

        #endregion
    }
}
