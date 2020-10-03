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
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using Hyperledger.Aries.Routing;
using mikoba.CoreImplementations;
using mikoba.Extensions;
using mikoba.Services;
using mikoba.ViewModels.Components;
using mikoba.ViewModels.SSI;
using ReactiveUI;
using Xamarin.Essentials;
using Xamarin.Forms;
using CredentialPreviewAttribute = mikoba.ViewModels.Components.CredentialPreviewAttribute;

namespace mikoba.ViewModels.Pages
{
    public class EntryHubPageViewModel : KivaBaseViewModel
    {
        public EntryHubPageViewModel(
            INavigationService navigationService,
            // IConnectionService connectionService,
            ICredentialService credentialService,
            // IAgentProvider contextProvider,
            // IActionDispatcher actionDispatcher,
            IEventAggregator eventAggregator)
            : base("Hub Page", navigationService)
        {
            // _connectionService = connectionService;
            // _contextProvider = contextProvider;
            // _contextProvider = contextProvider;
            // _credentialService = credentialService;
            // _actionDispatcher = actionDispatcher;
            _eventAggregator = eventAggregator;
        }

        #region Services

        private MediatorTimerService _mediatorTimer;
        private readonly IConnectionService _connectionService;
        private readonly ICredentialService _credentialService;
        private readonly IAgentProvider _contextProvider;
        private readonly IEventAggregator _eventAggregator;
        private readonly IActionDispatcher _actionDispatcher;

        #endregion

        #region Commands

        public ICommand RemoveConnectionCommand => new Command(async () =>
        {
            var context = await _contextProvider.GetContextAsync();
            var result = await _connectionService.DeleteAsync(context, Entry.Connection.Record.Id);
            if (result)
            {
                _mediatorTimer.Pause();
                _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.ConnectionsUpdated});
                await NavigationService.NavigateBackAsync();
            }
            else
            {
                _mediatorTimer.Pause();
                await NavigationService.NavigateBackAsync();
            }
        });

        public ICommand RemoveCredentialCommand => new Command(async () =>
        {
            var context = await _contextProvider.GetContextAsync();
            await _credentialService.DeleteCredentialAsync(context, _credential._credential.Id);
            _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.ConnectionsUpdated});
            await NavigationService.NavigateBackAsync();
        });

        public ICommand GoBackCommand => new Command(async () => { await NavigationService.NavigateBackAsync(); });

        #endregion

        #region UI Properties

        private SSICredentialViewModel _credential;

        public SSICredentialViewModel Credential
        {
            get => _credential;
            set => this.RaiseAndSetIfChanged(ref _credential, value);
        }

        private EntryViewModel _entry;

        public EntryViewModel Entry
        {
            get => _entry;
            set => this.RaiseAndSetIfChanged(ref _entry, value);
        }

        private ImageSource _photoAttach;

        public ImageSource PhotoAttach
        {
            get => _photoAttach;
            set => this.RaiseAndSetIfChanged(ref _photoAttach, value);
        }

        private RangeEnabledObservableCollection<CredentialPreviewAttribute> _attributes =
            new RangeEnabledObservableCollection<CredentialPreviewAttribute>();

        public RangeEnabledObservableCollection<CredentialPreviewAttribute> Attributes
        {
            get => _attributes;
            set => this.RaiseAndSetIfChanged(ref _attributes, value);
        }


        private RangeEnabledObservableCollection<string> _logs =
            new RangeEnabledObservableCollection<string>();

        public RangeEnabledObservableCollection<string> Logs
        {
            get => _logs;
            set => this.RaiseAndSetIfChanged(ref _logs, value);
        }

        private bool _hasCredential = false;

        public bool HasCredential
        {
            get => _hasCredential;
            set => this.RaiseAndSetIfChanged(ref _hasCredential, value);
        }

        private bool _hasConnection = false;

        public bool HasConnection
        {
            get => _hasConnection;
            set => this.RaiseAndSetIfChanged(ref _hasConnection, value);
        }

        #endregion

        #region Work

        private async void checkForWalletChanges()
        {
            // Device.BeginInvokeOnMainThread(
            //     async () =>
            //     {
            //         _mediatorTimer.Pause();
            //         var context = await _contextProvider.GetContextAsync();
            //         var results = await _edgeClientService.FetchInboxAsync(context);
            //         var credentialsRecords = await _credentialService.ListAsync(context);
            //         SSICredentialViewModel lastCredential = null;
            //         foreach (var credentialRecord in credentialsRecords)
            //         {
            //             var credential = new SSICredentialViewModel(credentialRecord);
            //             if (!credential.IsAccepted)
            //             {
            //                 lastCredential = credential;
            //                 break;
            //             }
            //         }
            //
            //         if (lastCredential != null)
            //         {
            //             await NavigationService.NavigateToAsync<CredentialOfferPageViewModel>(lastCredential,
            //                 NavigationType.Modal);
            //             _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.ConnectionsUpdated});
            //         }
            //         else
            //         {
            //             _mediatorTimer.Start();
            //         }
            //     });
        }

        private async void CheckMediator()
        {
            // _mediatorTimer.Pause();
            // var context = await _contextProvider.GetContextAsync();
            // var results = await _edgeClientService.FetchInboxAsync(context);
            // var itemsToDelete = new List<string>();
            // foreach (var item in results.unprocessedItems)
            // {
            //     Console.WriteLine(item.Data);
            //     if (!Preferences.ContainsKey(item.Id))
            //     {
            //         var message = await MessageDecoder.ProcessPackedMessage(context.Wallet, item, null);
            //         if (message != null)
            //         {
            //             Device.BeginInvokeOnMainThread(
            //                 async () => { await _actionDispatcher.DispatchMessage(message); });
            //         }
            //         else
            //         {
            //             Preferences.Set(item.Id, false);
            //         }
            //     }
            // }

            //TODO: Not supported by Mediator it seems.
            //Asked question in community and StackOverflow
            // if (itemsToDelete.Any())
            // {
            // var deleteMessage = new DeleteInboxItemsMessage() {InboxItemIds = itemsToDelete};
            // var response =
            //     await _messageService.SendReceiveAsync(context.Wallet, deleteMessage, this.Entry.Connection.Record);
            // //     Console.WriteLine(response.Payload);  
            // // }

            // _mediatorTimer.Start();
        }

        public override Task InitializeAsync(object navigationData)
        {
            // var agent = await _contextProvider.GetAgentAsync();
            // agent.Handlers.Add(new MikobaProofHandler(this._proofService));

            // if (navigationData is EntryViewModel entry)
            // {
            //     Entry = entry;
            //     Credential = entry.Credential;
            //     if (Credential != null)
            //     {
            //         HasCredential = true;
            //         var attributes = new List<CredentialPreviewAttribute>();
            //         foreach (var attribute in Credential.Attributes)
            //         {
            //             var allowedFiels = new String[]
            //                 {"nationalId", "photo~attach", "dateOfBirth", "firstName", "lastName"};
            //             if (!allowedFiels.Contains(attribute.Name)) continue;
            //             if (attribute.Name.Contains("photo~") && PhotoAttach == null)
            //             {
            //                 PhotoAttach = Xamarin.Forms.ImageSource.FromStream(
            //                     () => new MemoryStream(Convert.FromBase64String(attribute.Value.ToString())));
            //             }
            //             else
            //             {
            //                 attributes.Add(new CredentialPreviewAttribute()
            //                 {
            //                     Name = attribute.Name,
            //                     Value = attribute.Value.ToString(),
            //                 });
            //             }
            //         }
            //
            //         Attributes = new RangeEnabledObservableCollection<CredentialPreviewAttribute>();
            //         Attributes.AddRange(attributes);
            //     }
            //     else
            //     {
            //         HasConnection = true;
            //         _mediatorTimer = new MediatorTimerService(this.CheckMediator);
            //         _mediatorTimer.Start();
            //     }
            // }

            return base.InitializeAsync(navigationData);
        }

        #endregion
    }
}
