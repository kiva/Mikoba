using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.Discovery;
using Hyperledger.Aries.Features.TrustPing;
using mikoba.Extensions;
using mikoba.Services;
using ReactiveUI;
using Xamarin.Forms;

namespace mikoba.ViewModels.SSI
{
    public class SSIConnectionViewModel : MikobaBaseViewModel
    {
        public SSIConnectionViewModel(INavigationService navigationService,
                                   IAgentProvider agentContextProvider,
                                   IMessageService messageService,
                                   IDiscoveryService discoveryService,
                                   IConnectionService connectionService,
                                   IEventAggregator eventAggregator,
                                   ConnectionRecord record) :
                                   base(nameof(SSIConnectionViewModel),
                                       navigationService)
        {
            _agentContextProvider = agentContextProvider;
            _messageService = messageService;
            _discoveryService = discoveryService;
            _connectionService = connectionService;
            _eventAggregator = eventAggregator;

            Record = record;
            MyDid = Record.MyDid;
            TheirDid = Record.TheirDid;
            ConnectionName = Record.Alias?.Name;
            ConnectionSubtitle = $"{Record.State:G}";
            ConnectionImageUrl = Record.Alias?.ImageUrl;
        }
        
        public ConnectionRecord Record { get; private set; }
        
        #region Services
        private readonly IAgentProvider _agentContextProvider;
        private readonly IMessageService _messageService;
        private readonly IDiscoveryService _discoveryService;
        private readonly IConnectionService _connectionService;
        private readonly IEventAggregator _eventAggregator;
        #endregion

        public override async Task InitializeAsync(object navigationData)
        {
            await RefreshTransactions();
            await base.InitializeAsync(navigationData);
        }

        public async Task RefreshTransactions()
        {
            RefreshingTransactions = true;

            var context = await _agentContextProvider.GetContextAsync();
            var message = _discoveryService.CreateQuery(context, "*");

            DiscoveryDiscloseMessage protocols = null;

            try
            {
                var response = await _messageService.SendReceiveAsync(context.Wallet, message, Record) as UnpackedMessageContext;
                protocols = response.GetMessage<DiscoveryDiscloseMessage>();
            }
            catch (Exception)
            {
                //Swallow exception
                //TODO more granular error protection
            }

            IList<TransactionItem> transactions = new List<TransactionItem>();

            Transactions.Clear();

            if (protocols == null)
            {
                HasTransactions = false;
                RefreshingTransactions = false;
                return;
            }

            // foreach (var protocol in protocols.Protocols)
            // {
            //     switch (protocol.ProtocolId)
            //     {
            //         case MessageTypes.TrustPingMessageType:
            //             transactions.Add(new TransactionItem()
            //             {
            //                 Title = "Trust Ping",
            //                 Subtitle = "Version 1.0",
            //                 PrimaryActionTitle = "Ping",
            //                 PrimaryActionCommand = new Command(async () =>
            //                 {
            //                     await PingConnectionAsync();
            //                 }, () => true),
            //                 Type = TransactionItemType.Action.ToString("G")
            //             });
            //             break;
            //     }
            // }

            Transactions.InsertRange(transactions);
            HasTransactions = transactions.Any();

            RefreshingTransactions = false;
        }

        public async Task PingConnectionAsync()
        {
            // var dialog = UserDialogs.Instance.Loading("Pinging");
            var context = await _agentContextProvider.GetContextAsync();
            var message = new TrustPingMessage
            {
                ResponseRequested = true
            };
            
            try
            {
                var response = await _messageService.SendReceiveAsync(context.Wallet, message, Record) as UnpackedMessageContext;
                var trustPingResponse = response.GetMessage<TrustPingResponseMessage>();
            }
            catch (Exception)
            {
                //Swallow exception
                //TODO more granular error protection
            }

            // if (dialog.IsShowing)
            // {
            //     dialog.Hide();
            //     dialog.Dispose();
            // }

            // DialogService.Alert(
            //         success ?
            //         "Ping Response Recieved" :
            //         "No Ping Response Recieved"
            //     );
        }

        #region Commands
        public ICommand NavigateBackCommand => new Command(async () =>
        {
            await NavigationService.NavigateBackAsync();
        });

        public ICommand DeleteConnectionCommand => new Command(async () =>
        {
            // var dialog = DialogService.Loading("Deleting");
            
            var context = await _agentContextProvider.GetContextAsync();
            await _connectionService.DeleteAsync(context, Record.Id);
            
            _eventAggregator.Publish(new CoreDispatchedEvent() { Type = DispatchType.ConnectionsUpdated });
            
            // if (dialog.IsShowing)
            // {
            //     dialog.Hide();
            //     dialog.Dispose();
            // }
            
            await NavigationService.NavigateBackAsync();
        });

        public ICommand RefreshTransactionsCommand => new Command(async () => await RefreshTransactions());
        #endregion

        #region UI Properties
        private string _connectionName;
        public string ConnectionName
        {
            get => _connectionName;
            set => this.RaiseAndSetIfChanged(ref _connectionName, value);
        }

        private string _myDid;
        public string MyDid
        {
            get => _myDid;
            set => this.RaiseAndSetIfChanged(ref _myDid, value);
        }

        private string _theirDid;
        public string TheirDid
        {
            get => _theirDid;
            set => this.RaiseAndSetIfChanged(ref _theirDid, value);
        }

        private string _connectionImageUrl;
        public string ConnectionImageUrl
        {
            get => _connectionImageUrl;
            set => this.RaiseAndSetIfChanged(ref _connectionImageUrl, value);
        }

        private string _connectionSubtitle = "Lorem ipsum dolor sit amet";
        public string ConnectionSubtitle
        {
            get => _connectionSubtitle;
            set => this.RaiseAndSetIfChanged(ref _connectionSubtitle, value);
        }

        private RangeEnabledObservableCollection<TransactionItem> _transactions = new RangeEnabledObservableCollection<TransactionItem>();
        public RangeEnabledObservableCollection<TransactionItem> Transactions
        {
            get => _transactions;
            set => this.RaiseAndSetIfChanged(ref _transactions, value);
        }

        private bool _refreshingTransactions;
        public bool RefreshingTransactions
        {
            get => _refreshingTransactions;
            set => this.RaiseAndSetIfChanged(ref _refreshingTransactions, value);
        }

        private bool _hasTransactions;
        public bool HasTransactions
        {
            get => _hasTransactions;
            set => this.RaiseAndSetIfChanged(ref _hasTransactions, value);
        }
        #endregion
    }
}
