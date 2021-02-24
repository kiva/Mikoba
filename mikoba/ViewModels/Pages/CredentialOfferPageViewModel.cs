using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using DynamicData;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Indy.AnonCredsApi;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using mikoba.Extensions;
using mikoba.Services;
using mikoba.UI.Helpers;
using mikoba.ViewModels.SSI;
using ReactiveUI;
using Sentry;
using Sentry.Protocol;
using West.Extensions.Xamarin;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using INavigationService = mikoba.Services.INavigationService;

namespace mikoba.ViewModels.Pages
{
    public class CredentialOfferPageViewModel : MikobaBaseViewModel
    {
        private static readonly string[] AllowedFields =
            {"nationalId", "photo~attach", "dateOfBirth", "birthDate", "firstName", "lastName"};

        public CredentialOfferPageViewModel(INavigationService navigationService,
            IMessageService messageService,
            IAgentProvider contextProvider,
            ICredentialService credentialService,
            IUserDialogs dialogService,
            IEventAggregator eventAggregator)
            : base("Accept Invitation", navigationService)
        {
            _contextProvider = contextProvider;
            _messageService = messageService;
            _contextProvider = contextProvider;
            _eventAggregator = eventAggregator;
            _credentialService = credentialService;
            _dialogService = dialogService;
        }
        
        private CredentialOfferTransport _transport;

        #region Services

        private readonly IUserDialogs _dialogService;
        private readonly ICredentialService _credentialService;
        private readonly IMessageService _messageService;
        private readonly IAgentProvider _contextProvider;
        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Commands

        public ICommand CloseReceiptCommand => new Command(async () =>
        {
            _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.ConnectionsUpdated});
            await NavigationService.PopModalAsync();
        });

        public ICommand AcceptCommand => new Command(async () =>
        {
            try
            {
                var context = await _contextProvider.GetContextAsync();

                Tracking.TrackEvent("Click Accept Credential");
                Analytics.TrackEvent("Click Accept Credential");
                var (request, _) = await _credentialService.CreateRequestAsync(context, _transport.Record.Id);
                await _messageService.SendAsync(context.Wallet, request, _transport.MessageContext.Connection);

                await NavigationService.PopModalAsync();

                _eventAggregator.Publish(new CoreDispatchedEvent()
                {
                    Type = DispatchType.CredentialAccepted,
                    Data = _transport.Record.Id
                });

                Tracking.TrackEvent("Save Credential");
            }
            catch (Exception ex)
            {
                Tracking.TrackException(ex, "Failed to Save Credential");
                _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.CredentialAcceptanceFailed});
                await _dialogService.AlertAsync("Can't add credential", ex.Message, "OK");
            }
        });

        public ICommand DeclineCommand => new Command(async () =>
        {
            Tracking.TrackEvent("Decline Credential");
            _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.CredentialDeclined});
            await NavigationService.PopModalAsync();
        });

        #endregion

        #region UI Properties

        private bool _showReceipt;

        public bool ShowReceipt
        {
            get => _showReceipt;
            set => this.RaiseAndSetIfChanged(ref _showReceipt, value);
        }


        private string _connectionEstablishedText;

        public string ConnectionEstablishedText
        {
            get => _connectionEstablishedText;
            set => this.RaiseAndSetIfChanged(ref _connectionEstablishedText, value);
        }

        private string _offerByText;

        public string OfferByText
        {
            get => _offerByText;
            set => this.RaiseAndSetIfChanged(ref _offerByText, value);
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
                    // TODO: "No image found" placeholder
                    if (attribute.Name.Contains("~") && PhotoAttach == null)
                    {
                        //TODO: Ensure that in this portion of the code there isn't a notion of base64, just bytes.
                        string value = PhotoAttachParser.ReturnAttachment(attribute.Value.ToString());
                        PhotoAttach = ImageSource.FromStream(() =>
                            new MemoryStream(Convert.FromBase64String(value)));
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
            
            //TODO: "Jul 12, 2020 8:08 pm";
            ConnectionEstablishedText = DateTime.Now.ToLongDateString();
            OfferByText = "Offer by Kiva";
            
            return base.InitializeAsync(navigationData);
        }

        #endregion
    }
}
