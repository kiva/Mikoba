using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Routing;
using mikoba.Extensions;
using mikoba.Services;
using mikoba.UI.Helpers;
using mikoba.ViewModels.Components;
using mikoba.ViewModels.SSI;
using ReactiveUI;
using Xamarin.Forms;
using Acr.UserDialogs;
using INavigationService = mikoba.Services.INavigationService;

namespace mikoba.ViewModels.Pages
{
    public class EntryHubPageViewModel : MikobaBaseViewModel
    {
        public EntryHubPageViewModel(
            INavigationService navigationService,
            IConnectionService connectionService,
            ICredentialService credentialService,
            IEdgeClientService edgeClientService,
            IAgentProvider contextProvider,
            IEventAggregator eventAggregator,
            IUserDialogs userDialogs
        )
            : base("Hub Page", navigationService)
        {
            _connectionService = connectionService;
            _contextProvider = contextProvider;
            _contextProvider = contextProvider;
            _credentialService = credentialService;
            _eventAggregator = eventAggregator;
            _edgeClientService = edgeClientService;
            _userDialogs = userDialogs;
        }

        #region Services

        private readonly IConnectionService _connectionService;
        private readonly ICredentialService _credentialService;
        private readonly IAgentProvider _contextProvider;
        private readonly IEventAggregator _eventAggregator;
        private readonly IEdgeClientService _edgeClientService;
        private readonly IUserDialogs _userDialogs;

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
            try
            {
                var userConfirmation = await _userDialogs.ConfirmAsync("Are you sure you want to remove this credential?",
                    "Confirmation", "Yes", "No, Cancel");
                if (!userConfirmation)
                {
                    return;
                }
                var context = await _contextProvider.GetContextAsync();
                await _credentialService.DeleteCredentialAsync(context, _credential._credential.Id);
                _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.ConnectionsUpdated});
                _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.CredentialRemoved});
                await NavigationService.NavigateBackAsync();
            }
            catch(Exception ex)
            {
                Tracking.TrackException(ex, "Remove Credential");
            }
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
                            string value = PhotoAttachParser.ReturnAttachment(attribute.Value.ToString());
                            PhotoAttach = ImageSource.FromStream(() =>
                                new MemoryStream(Convert.FromBase64String(value)));
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
