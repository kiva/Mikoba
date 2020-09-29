using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using DynamicData;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Agents.Edge;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using mikoba.Extensions;
using mikoba.Services;
using mikoba.UI.ViewModels;
using mikoba.ViewModels.SSI;
using ReactiveUI;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages
{
    public class CredentialOfferPageViewModel : KivaBaseViewModel
    {
        public CredentialOfferPageViewModel(INavigationService navigationService,
                                     IConnectionService connectionService,
                                     IMessageService messageService,
                                     IAgentProvider contextProvider,
                                     ICredentialService credentialsService,
                                         IEventAggregator eventAggregator)
                                     : base("Accept Invitation", navigationService)
        {
            _connectionService = connectionService;
            _contextProvider = contextProvider;
            _messageService = messageService;
            _contextProvider = contextProvider;
            _eventAggregator = eventAggregator;
            _credentialsService = credentialsService;
        }
        
        private CredentialOfferMessage _offerMessage;

        #region Services 
        private readonly ICredentialService _credentialsService;
        private readonly IProvisioningService _provisioningService;
        private readonly IConnectionService _connectionService;
        private readonly IMessageService _messageService;
        private readonly IAgentProvider _contextProvider;
        private readonly IEventAggregator _eventAggregator;
        #endregion
        
        #region Commands
        public ICommand AcceptCommand => new Command(async () =>
        {
            var context = await _contextProvider.GetContextAsync();
            try
            {
                var identifier = await _credentialsService.ProcessOfferAsync(context, _offerMessage, new ConnectionRecord());
                _eventAggregator.Publish(new CoreDispatchedEvent() { Type = DispatchType.ConnectionsUpdated });
            }
            finally
            {
                await NavigationService.PopModalAsync();
            }
        });

        public ICommand RejectCommand => new Command(async () => await NavigationService.PopModalAsync());

        #endregion

        #region UI Properties
        private ImageSource _photoAttach;
        public ImageSource PhotoAttach
        {
            get => _photoAttach;
            set => this.RaiseAndSetIfChanged(ref _photoAttach, value);
        }
        private RangeEnabledObservableCollection<CredentialPreviewAttribute> _previewAttributes =
            new RangeEnabledObservableCollection<CredentialPreviewAttribute>();

        public RangeEnabledObservableCollection<CredentialPreviewAttribute> PreviewAttributes
        {
            get => _previewAttributes;
            set => this.RaiseAndSetIfChanged(ref _previewAttributes, value);
        }
        #endregion
        
        #region Work
        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is CredentialOfferMessage offer)
            {
                var previewAttributes = new List<CredentialPreviewAttribute>();
                foreach (var attribute in offer.CredentialPreview.Attributes)
                {
                    if (attribute.Name.Contains("~") && PhotoAttach == null)
                    {
                        PhotoAttach = Xamarin.Forms.ImageSource.FromStream(
                            () => new MemoryStream(Convert.FromBase64String(attribute.Value.ToString())));    
                    }
                    else
                    {
                        previewAttributes.Add(new CredentialPreviewAttribute()
                        {
                            Name = attribute.Name,
                            Value = attribute.Value,
                            MimeType = attribute.MimeType,
                        });
                    }
                }
                PreviewAttributes = new RangeEnabledObservableCollection<CredentialPreviewAttribute>();
                PreviewAttributes.AddRange(previewAttributes);
                _offerMessage = offer;
            }
            return base.InitializeAsync(navigationData);
        }
        #endregion
    }
}
