using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using mikoba.Extensions;
using mikoba.Services;
using mikoba.ViewModels.Components;
using mikoba.ViewModels.SSI;
using ReactiveUI;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages
{
    public class EntryHubPageViewModel : KivaBaseViewModel
    {
        public EntryHubPageViewModel(INavigationService navigationService,
                                     IConnectionService connectionService,
                                     IMessageService messageService,
                                     IAgentProvider contextProvider,
                                     IEventAggregator eventAggregator)
                                     : base("Accept Invitation", navigationService)
        {
            _connectionService = connectionService;
            _contextProvider = contextProvider;
            _messageService = messageService;
            _contextProvider = contextProvider;
            _eventAggregator = eventAggregator;
        }

          #region Services
        private readonly IConnectionService _connectionService;
        private readonly IMessageService _messageService;
        private readonly IAgentProvider _contextProvider;
        private readonly IEventAggregator _eventAggregator;
        #endregion
        
        #region Commands
        public ICommand RemoveConnectionCommand => new Command(async () =>
        {
            var context = await _contextProvider.GetContextAsync();
            var result = await _connectionService.DeleteAsync(context, Entry.Connection.Record.Id);
            if (result)
            {
                _eventAggregator.Publish(new CoreDispatchedEvent() { Type = DispatchType.ConnectionsUpdated });
                await Task.Delay(1000); //Allow some time for event propagation.
                await NavigationService.NavigateBackAsync();
            }
            else
            {
                await NavigationService.NavigateBackAsync();
            }
        }); 
        
        public ICommand GoBackCommand => new Command(async () =>
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
                }
            }
            return base.InitializeAsync(navigationData);
        }
        #endregion
    }
}
