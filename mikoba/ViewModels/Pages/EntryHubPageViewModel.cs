using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using DynamicData;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using Hyperledger.Aries.Routing;
using mikoba.Extensions;
using mikoba.Services;
using mikoba.ViewModels.Components;
using mikoba.ViewModels.SSI;
using ReactiveUI;
using Xamarin.Forms;
using CredentialPreviewAttribute = mikoba.ViewModels.Components.CredentialPreviewAttribute;

namespace mikoba.ViewModels.Pages
{
    public class EntryHubPageViewModel : KivaBaseViewModel
    {
        public EntryHubPageViewModel(INavigationService navigationService,
            IConnectionService connectionService,
            IMessageService messageService,
            IAgentProvider contextProvider,
            IActionDispatcher actionDispatcher,
            IEventAggregator eventAggregator)
            : base("Hub Page", navigationService)
        {
            _connectionService = connectionService;
            _contextProvider = contextProvider;
            _messageService = messageService;
            _contextProvider = contextProvider;
            _eventAggregator = eventAggregator;
            _actionDispatcher = actionDispatcher;
        }

        #region Services

        private MediatorTimerService _mediatorTimer;
        private readonly IConnectionService _connectionService;
        private readonly IMessageService _messageService;
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
                _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.ConnectionsUpdated});
                await NavigationService.NavigateBackAsync();
            }
            else
            {
                await NavigationService.NavigateBackAsync();
            }
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

        private bool _hasCredential = false;

        public bool HasCredential
        {
            get => _hasCredential;
            set => this.RaiseAndSetIfChanged(ref _hasCredential, value);
        }

        #endregion

        #region Work

        private async void CheckMediator()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var context = await App.Container.Resolve<IAgentProvider>().GetContextAsync();
                var results = await App.Container.Resolve<IEdgeClientService>().FetchInboxAsync(context);
                foreach (var item in results.unprocessedItems)
                {
                    var message = await MessageDecoder.ParseMessageAsync(item.Data);
                    _actionDispatcher.DispatchMessage(message);
                }
            });
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is EntryViewModel entry)
            {
                Entry = entry;
                Credential = entry.Credential;
                if (Credential != null)
                {
                    HasCredential = true;
                    var attributes = new List<CredentialPreviewAttribute>();
                    foreach (var attribute in Credential.Attributes)
                    {
                        if (attribute.Name.Contains("~") && PhotoAttach == null)
                        {
                            PhotoAttach = Xamarin.Forms.ImageSource.FromStream(
                                () => new MemoryStream(Convert.FromBase64String(attribute.Value.ToString())));
                        }
                        else
                        {
                            attributes.Add(new CredentialPreviewAttribute()
                            {
                                Name = attribute.Name,
                                Value = attribute.Value.ToString(),
                            });
                        }
                    }

                    Attributes = new RangeEnabledObservableCollection<CredentialPreviewAttribute>();
                    Attributes.AddRange(attributes);
                }
                else
                {
                    HasCredential = false;
                    _mediatorTimer = new MediatorTimerService(this.CheckMediator);
                    _mediatorTimer.Start();
                }
            }

            return base.InitializeAsync(navigationData);
        }

        #endregion
    }
}
