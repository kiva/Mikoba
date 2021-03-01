using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Autofac;
using DynamicData;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using Hyperledger.Aries.Routing;
using Microsoft.AppCenter.Crashes;
using mikoba.Extensions;
using mikoba.Services;
using mikoba.UI.ViewModels;
using mikoba.ViewModels.Components;
using mikoba.ViewModels.SSI;
using ReactiveUI;
using Sentry;
using Xamarin.Essentials;
using Xamarin.Forms;
using Acr.UserDialogs;
using INavigationService = mikoba.Services.INavigationService;

namespace mikoba.ViewModels.Pages
{
    public class WalletPageViewModel : MikobaBaseViewModel
    {
        private Timer timer;

        public WalletPageViewModel(
            INavigationService navigationService,
            IConnectionService connectionService,
            ICredentialService credentialService,
            IEdgeClientService edgeClientService,
            IAgentProvider agentContextProvider,
            IEventAggregator eventAggregator,
            IUserDialogs dialogService,
            ILifetimeScope scope) :
            base("Wallet Page", navigationService)
        {
            _credentialService = credentialService;
            _agentContextProvider = agentContextProvider;
            _connectionService = connectionService;
            _eventAggregator = eventAggregator;
            _edgeClientService = edgeClientService;
            _dialogService = dialogService;
            _scope = scope;
            _mediatorTimer = new MediatorTimerService(this.CheckMediator);
        }

        private void CheckMediator()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    _mediatorTimer.Pause();
                    var context = await _agentContextProvider.GetContextAsync();
                    await _edgeClientService.FetchInboxAsync(context);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    SentrySdk.CaptureException(ex);
                }
                finally
                {
                    await RefreshData();
                    _mediatorTimer.Start();
                }
            });
        }

        private readonly MediatorTimerService _mediatorTimer;


        #region Services

        private readonly IEventAggregator _eventAggregator;
        private readonly IEdgeClientService _edgeClientService;
        private readonly ICredentialService _credentialService;
        private readonly IConnectionService _connectionService;
        private readonly IAgentProvider _agentContextProvider;
        private readonly ILifetimeScope _scope;
        private readonly IUserDialogs _dialogService;

        #endregion

        #region UI Properties

        private string _welcomeText;

        public string WelcomeText
        {
            get => _welcomeText;
            set => this.RaiseAndSetIfChanged(ref _welcomeText, value);
        }

        private string _lastCredentialCreatedId;
        
        private string _notificationText;

        public string NotificationText
        {
            get => _notificationText;
            set => this.RaiseAndSetIfChanged(ref _notificationText, value);
        }

        private bool _isRefreshing;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => this.RaiseAndSetIfChanged(ref _isRefreshing, value);
        }

        private bool _hasContent;

        public bool HasContent
        {
            get => _hasContent;
            set => this.RaiseAndSetIfChanged(ref _hasContent, value);
        }

        private RangeEnabledObservableCollection<SSICredentialViewModel> _credentials =
            new RangeEnabledObservableCollection<SSICredentialViewModel>();

        public RangeEnabledObservableCollection<SSICredentialViewModel> Credentials
        {
            get => _credentials;
            set => this.RaiseAndSetIfChanged(ref _credentials, value);
        }

        private RangeEnabledObservableCollection<SSIConnectionViewModel> _connections =
            new RangeEnabledObservableCollection<SSIConnectionViewModel>();

        public RangeEnabledObservableCollection<SSIConnectionViewModel> Connections
        {
            get => _connections;
            set => this.RaiseAndSetIfChanged(ref _connections, value);
        }


        private RangeEnabledObservableCollection<EntryViewModel> _entries =
            new RangeEnabledObservableCollection<EntryViewModel>();

        public RangeEnabledObservableCollection<EntryViewModel> Entries
        {
            get => _entries;
            set => this.RaiseAndSetIfChanged(ref _entries, value);
        }

        #endregion

        #region Lifecycle

        public override async Task InitializeAsync(object navigationData)
        {
            SentrySdk.CaptureEvent(
                new SentryEvent()
                {
                    Message = "Load Home Page"
                });

            Preferences.Set(AppConstant.LocalWalletFirstView, false);
            IsRefreshing = true;
            await RefreshEntries();
            WelcomeText =
                $"Hello {Preferences.Get(AppConstant.FullName, "")}, welcome to your new Wallet.  Get started by receiving your first ID.";
            IsRefreshing = false;
            _eventAggregator.GetEventByType<CoreDispatchedEvent>()
                .Subscribe(async _ =>
                {
                    if (_.Type == DispatchType.ConnectionCreated)
                    {
                        NotificationText = "Kiva can now send you requests.";
                    }
                    else if (_.Type == DispatchType.CredentialAccepted)
                    {
                        NotificationText = "Credential accepted.";
                        if (!string.IsNullOrWhiteSpace(_.Data))
                        {
                            _lastCredentialCreatedId = _.Data;
                        }
                    }
                    else if (_.Type == DispatchType.CredentialDeclined)
                    {
                        NotificationText = "Failed to save credential.";
                    }
                    else if (_.Type == DispatchType.CredentialAcceptanceFailed)
                    {
                        NotificationText = "Credential declined.";
                    }
                    else if (_.Type == DispatchType.CredentialRemoved)
                    {
                        NotificationText = "Credential deleted.";
                    }
                    else if (_.Type == DispatchType.CredentialShared)
                    {
                        NotificationText = "Credential shared.";
                    }
                    else if (_.Type == DispatchType.CredentialShareFailed)
                    {
                        NotificationText = "Credential share failed.";
                    }
                    else if (_.Type == DispatchType.NotificationDismissed)
                    {
                        NotificationText = "";
                    }

                    await RefreshData();
                });
            _mediatorTimer.Start();

            await base.InitializeAsync(navigationData);
        }

        #endregion

        #region Commands

        public ICommand ScanCodeCommand
        {
            get { return new Command(async () => { await this.ScanInvite(); }); }
        }

        public ICommand RefreshCommand
        {
            get => new Command(async () => { await this.RefreshCredentials(); });
        }


        public ICommand SettingsCommand

        {
            get => new Command(async () => { await NavigationService.NavigateToAsync<SettingsPageViewModel>(); });
        }


        public ICommand RecoverWalletCommand
        {
            get => new Command(async () =>
            {
                await _dialogService.AlertAsync("Not implemented!", "Notice", "OK");
            });
        }

        #endregion

        #region Work

        public async Task RefreshData()
        {
            IsRefreshing = true;
            await this.RefreshEntries();
            IsRefreshing = false;
            await ExecutePostCredentialActions();
        }

        private async Task ExecutePostCredentialActions()
        {
            if (!string.IsNullOrWhiteSpace(_lastCredentialCreatedId))
            {
                var credentialToOpen = Entries.FirstOrDefault(x =>
                {
                    return x.HasCredential && x.Credential._credential.CredentialId == _lastCredentialCreatedId;
                });
                if (credentialToOpen != null)
                {
                    _lastCredentialCreatedId = null;
                    await NavigationService.NavigateToAsync<EntryHubPageViewModel>(credentialToOpen);
                }
            }
        }

        public async Task RefreshCredentials()
        {
            var context = await _agentContextProvider.GetContextAsync();
            var credentialsRecords = await _credentialService.ListAsync(context);

            var credentialsVms = new List<SSICredentialViewModel>();
            foreach (var credentialRecord in credentialsRecords)
            {
                if (credentialRecord.CredentialId == null) continue;
                SSICredentialViewModel credential =
                    _scope.Resolve<SSICredentialViewModel>(new NamedParameter("credential", credentialRecord));
                credentialsVms.Add(credential);
            }

            Credentials.Clear();
            Credentials.InsertRange(credentialsVms);
        }

        //TODO: Move this to upcoming history view.
        // ReSharper disable once UnusedMember.Global
        public async Task RefreshConnections()
        {
            var context = await _agentContextProvider.GetContextAsync();
            var records = await _connectionService.ListAsync(context);

            IList<SSIConnectionViewModel> connectionVms = new List<SSIConnectionViewModel>();
            foreach (var record in records)
            {
                if (record.Alias != null)
                {
                    var connection = _scope.Resolve<SSIConnectionViewModel>(new NamedParameter("record", record));
                    connectionVms.Add(connection);
                }
            }

            Connections.Clear();
            Connections.InsertRange(connectionVms);
        }

        private async Task RefreshEntries()
        {
            await this.RefreshCredentials();
            var entries = new List<EntryViewModel>();
            foreach (var credential in Credentials)
            {
                var entry = _scope.Resolve<EntryViewModel>();
                entry.Credential = credential;
                entry.Setup();
                entries.Add(entry);
            }

            Entries.Clear();
            Entries.AddRange(entries);
            HasContent = Entries.Any();
        }

        private async Task ScanInvite()
        {
            var expectedFormat = ZXing.BarcodeFormat.QR_CODE;
            var opts = new ZXing.Mobile.MobileBarcodeScanningOptions
                {PossibleFormats = new List<ZXing.BarcodeFormat> {expectedFormat}};

            var scanner = new ZXing.Mobile.MobileBarcodeScanner();

            var result = await scanner.Scan(opts);
            if (result == null) return;

            try
            {
                var message = await MessageDecoder.ParseMessageAsync(result.Text);
                if (message is ConnectionInvitationMessage)
                {
                    ConnectionInvitationMessage inviteMessage = (ConnectionInvitationMessage) message;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await NavigationService.NavigateToAsync<AcceptConnectionInviteViewModel>(inviteMessage,
                            NavigationType.Modal);
                    });
                }
                else if (message is CredentialOfferMessage)
                {
                    CredentialOfferMessage credentialOffer = (CredentialOfferMessage) message;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await NavigationService.NavigateToAsync<CredentialOfferPageViewModel>(credentialOffer,
                            NavigationType.Modal);
                    });
                }
                else if (message is RequestPresentationMessage)
                {
                    RequestPresentationMessage credentialRequest = (RequestPresentationMessage) message;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await NavigationService.NavigateToAsync<ProofRequestViewModel>(credentialRequest,
                            NavigationType.Modal);
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
