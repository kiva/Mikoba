using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
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
using Hyperledger.Aries.Storage;
using mikoba.CoreImplementations;
using mikoba.Extensions;
using mikoba.Services;
using mikoba.Tools;
using mikoba.ViewModels.Components;
using mikoba.ViewModels.SSI;
using Newtonsoft.Json;
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
            IConnectionService connectionService,
            ICredentialService credentialService,
            IEdgeClientService edgeClientService,
            IAgentProvider contextProvider,
            IEventAggregator eventAggregator
        )
            : base("Hub Page", navigationService)
        {
            _connectionService = connectionService;
            _contextProvider = contextProvider;
            _contextProvider = contextProvider;
            _credentialService = credentialService;
            _eventAggregator = eventAggregator;
            _edgeClientService = edgeClientService;
        }

        #region Services
        
        private readonly IConnectionService _connectionService;
        private readonly ICredentialService _credentialService;
        private readonly IAgentProvider _contextProvider;
        private readonly IEventAggregator _eventAggregator;
        private readonly IEdgeClientService _edgeClientService;

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

        public ICommand RemoveCredentialCommand => new Command(async () =>
        {
            var context = await _contextProvider.GetContextAsync();
            await _credentialService.DeleteCredentialAsync(context, _credential._credential.Id);
            _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.ConnectionsUpdated});
            _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.CredentialRemoved});
            await NavigationService.NavigateBackAsync();
        });

        public new ICommand GoBackCommand => new Command(async () =>
        {
            await NavigationService.NavigateBackAsync();
        });

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

        private RangeEnabledObservableCollection<SSICredentialAttribute> _attributes =
            new RangeEnabledObservableCollection<SSICredentialAttribute>();

        public RangeEnabledObservableCollection<SSICredentialAttribute> Attributes
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

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is EntryViewModel entry)
            {
                Entry = entry;
                Credential = entry.Credential;
                if (Credential != null)
                {
                    HasCredential = true;
                    HasConnection = false;
                    var attributes = new List<SSICredentialAttribute>();
                    foreach (var attribute in Credential.Attributes)
                    {
                        if (attribute.Name.Contains("~") && PhotoAttach == null)
                        {
                            PhotoAttach = Xamarin.Forms.ImageSource.FromStream(
                                () => new MemoryStream(CredentialTools.ProcessJSONImageField(attribute.Value.ToString())));
                        }
                        else
                        {
                            attributes.Add(new SSICredentialAttribute()
                            {
                                Name = attribute.Name,
                                Value = attribute.Value.ToString(),
                            });
                        }
                    }

                    Attributes = new RangeEnabledObservableCollection<SSICredentialAttribute>();
                    Attributes.AddRange(attributes);
                }
                else
                {
                    HasCredential = false;
                    HasConnection = true;
                }
            }

            return base.InitializeAsync(navigationData);
        }

        #endregion
    }
}
