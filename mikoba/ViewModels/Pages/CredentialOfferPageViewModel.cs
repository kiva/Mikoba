using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Indy.AnonCredsApi;
using mikoba.Extensions;
using mikoba.Services;
using mikoba.ViewModels.SSI;
using ReactiveUI;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages
{
    public class CredentialOfferPageViewModel : KivaBaseViewModel
    {
        private static readonly string[] AllowedFields =
            {"nationalId", "photo~attach", "dateOfBirth", "birthDate", "firstName", "lastName"};

        public CredentialOfferPageViewModel(INavigationService navigationService,
            IConnectionService connectionService,
            IMessageService messageService,
            IAgentProvider contextProvider,
            ICredentialService credentialService,
            IEventAggregator eventAggregator)
            : base("Accept Invitation", navigationService)
        {
            _connectionService = connectionService;
            _contextProvider = contextProvider;
            _messageService = messageService;
            _contextProvider = contextProvider;
            _eventAggregator = eventAggregator;
            _credentialService = credentialService;
        }

        private CredentialOfferMessage _offerMessage;
        private CredentialOfferTransport _transport;

        #region Services

        private readonly ICredentialService _credentialService;
        private readonly IProvisioningService _provisioningService;
        private readonly IConnectionService _connectionService;
        private readonly IMessageService _messageService;
        private readonly IAgentProvider _contextProvider;
        private readonly IEventAggregator _eventAggregator;
        private SSICredentialViewModel _ssiCredentialViewModel;

        #endregion

        #region Commands

        public ICommand CloseReceiptCommand => new Command(async () => { await NavigationService.PopModalAsync(); });

        public ICommand AcceptCommand => new Command(async () =>
        {
            var context = await _contextProvider.GetContextAsync();
            try
            {
                var (request, _) = await _credentialService.CreateRequestAsync(context, _transport.Record.Id);
                await _messageService.SendAsync(context.Wallet, request, _transport.MessageContext.Connection);
                // if (messageContext.ContextRecord is CredentialRecord newCredentialRecord)
                // {
                //     _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.ConnectionsUpdated});
                //     ShowReceipt = true;
                // } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        });

        public ICommand DeclineCommand => new Command(async () => { await NavigationService.PopModalAsync(); });

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

        private RangeEnabledObservableCollection<SSICredentialAttribute> _previewAttributes =
            new RangeEnabledObservableCollection<SSICredentialAttribute>();

        public RangeEnabledObservableCollection<SSICredentialAttribute> PreviewAttributes
        {
            get => _previewAttributes;
            set => this.RaiseAndSetIfChanged(ref _previewAttributes, value);
        }

        #endregion

        #region Work

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is CredentialOfferTransport transport)
            {
                _transport = transport;
                var previewAttributes = new List<SSICredentialAttribute>();
                foreach (var attribute in _transport.Message.CredentialPreview.Attributes)
                {
                    if (!AllowedFields.Contains(attribute.Name)) continue;
                    if (attribute.Name.Contains("~") && PhotoAttach == null)
                    {
                        PhotoAttach = ImageSource.FromStream(() =>
                            new MemoryStream(Convert.FromBase64String(attribute.Value.ToString())));
                    }
                    else
                    {
                        previewAttributes.Add(new SSICredentialAttribute()
                        {
                            Name = attribute.Name,
                            Value = attribute.Value,
                        });
                    }
                }
                PreviewAttributes = new RangeEnabledObservableCollection<SSICredentialAttribute>();
                PreviewAttributes.AddRange(previewAttributes);
            }

            return base.InitializeAsync(navigationData);
        }

        #endregion
    }
}
