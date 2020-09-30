using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using DynamicData;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Agents.Edge;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using mikoba.Extensions;
using mikoba.Services;
using mikoba.UI.ViewModels;
using mikoba.ViewModels.Components;
using mikoba.ViewModels.SSI;
using ReactiveUI;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages
{
    public class WalletPageViewModel : KivaBaseViewModel
    {
        public WalletPageViewModel(
            INavigationService navigationService,
            IConnectionService connectionService,
            ICredentialService credentialService,
            IAgentProvider agentContextProvider,
            IEventAggregator eventAggregator,
            ILifetimeScope scope) : base("Wallet Page", navigationService)
        {
            _credentialService = credentialService;
            _agentContextProvider = agentContextProvider;
            _connectionService = connectionService;
            _eventAggregator = eventAggregator;
            _scope = scope;
            // this.WhenAnyValue(x => x.SearchTerm)
            //     .Throttle(TimeSpan.FromMilliseconds(200))
            //     .InvokeCommand(RefreshCommand);
        }

        #region Services

        private readonly IEventAggregator _eventAggregator;
        private readonly ICredentialService _credentialService;
        private readonly IConnectionService _connectionService;
        private readonly IAgentProvider _agentContextProvider;
        private readonly ILifetimeScope _scope;

        #endregion

        #region UI Properties

        public ObservableCollection<WalletActionModel> WalletActions { get; set; }

        private string _welcomeText;

        public string WelcomeText
        {
            get => _welcomeText;
            set => this.RaiseAndSetIfChanged(ref _welcomeText, value);
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
            Preferences.Set(AppConstant.LocalWalletFirstView, false);
            IsRefreshing = true;
            await RefreshCredentials();
            await RefreshConnections();
            await RefreshEntries();
            WelcomeText =
                $"Hello {Preferences.Get(AppConstant.FullName, "Horacio")}, welcome to your new Wallet.  Get started by receiving your first ID.";
            IsRefreshing = false;
            _eventAggregator?.GetEventByType<CoreDispatchedEvent>()
                .Where(_ => _.Type == DispatchType.ConnectionsUpdated)
                .Subscribe(async _ => await RefreshData());
            await base.InitializeAsync(navigationData);
        }

        #endregion

        #region Commands

        public ICommand ScanCodeCommand
        {
            get { return new Command(async () =>
            {
                await this.ScanInvite();
            }); }
        }

        public ICommand RefreshCommand
        {
            get => new Command(async () => { await this.RefreshCredentials(); });
        }

        #endregion

        #region Work

        public async Task RefreshData()
        {
            IsRefreshing = true;
            await this.RefreshConnections();
            await this.RefreshCredentials();
            await this.RefreshEntries();
            HasContent = this.Entries.Any();
            IsRefreshing = false;
        }

        public async Task RefreshCredentials()
        {
            var context = await _agentContextProvider.GetContextAsync();
            var credentialsRecords = await _credentialService.ListAsync(context);

            IList<SSICredentialViewModel> credentialsVms = new List<SSICredentialViewModel>();
            foreach (var credentialRecord in credentialsRecords)
            {
                SSICredentialViewModel credential =
                    _scope.Resolve<SSICredentialViewModel>(new NamedParameter("credential", credentialRecord));
                credentialsVms.Add(credential);
            }

            Credentials.Clear();
            Credentials.InsertRange(credentialsVms);
        }

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

        public async Task RefreshEntries()
        {
            var entries = new List<EntryViewModel>();
            // foreach (var connection in Connections)
            // {
            //     var entry = _scope.Resolve<EntryViewModel>();
            //     entry.Connection = connection;
            //     entry.Setup();
            //     entries.Add(entry);
            // }

            foreach (var credential in Credentials)
            {
                var entry = _scope.Resolve<EntryViewModel>();
                entry.Credential = credential;
                entry.Setup();
                entries.Add(entry);
            }
            
            #if DEBUG
            
            #endif

            Entries.Clear();
            Entries.AddRange(entries);
            HasContent = Entries.Any();
        }

        public async Task ScanInvite()
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
                    ConnectionInvitationMessage inviteMessage = (ConnectionInvitationMessage)message;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await NavigationService.NavigateToAsync<AcceptConnectionInviteViewModel>(inviteMessage, NavigationType.Modal);
                    });
                }
                else if (message is CredentialOfferMessage)
                {
                    CredentialOfferMessage credentialOffer = (CredentialOfferMessage)message;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await NavigationService.NavigateToAsync<CredentialOfferPageViewModel>(credentialOffer, NavigationType.Modal);
                    });
                }
                else if (message is RequestPresentationMessage)
                {
                    RequestPresentationMessage credentialRequest = (RequestPresentationMessage)message;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await NavigationService.NavigateToAsync<CredentialRequestPageViewModel>(credentialRequest, NavigationType.Modal);
                    });
                }
                
            }
            catch (Exception)
            {
                
            }

           
        }

        #endregion
    }
}
