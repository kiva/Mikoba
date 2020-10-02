using System.Threading.Tasks;
using System.Windows.Input;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using mikoba.Extensions;
using mikoba.Services;
using ReactiveUI;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages
{
    public class CredentialRequestPageViewModel : KivaBaseViewModel
    {
        public CredentialRequestPageViewModel(
            INavigationService navigationService,
            IConnectionService connectionService,
            IMessageService messageService,
            IAgentProvider contextProvider,
            ICredentialService credentialsService,
            IEventAggregator eventAggregator)
            : base("Credential Request", navigationService)
        {
            _connectionService = connectionService;
            _contextProvider = contextProvider;
            _messageService = messageService;
            _contextProvider = contextProvider;
            _eventAggregator = eventAggregator;
            _credentialsService = credentialsService;
        }

        private RequestPresentationMessage _requestMessage;

        #region Services

        private readonly ICredentialService _credentialsService;
        private readonly IProvisioningService _provisioningService;
        private readonly IConnectionService _connectionService;
        private readonly IMessageService _messageService;
        private readonly IAgentProvider _contextProvider;
        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Commands

        public ICommand SubmitCommand => new Command(async () =>
        {
            var context = await _contextProvider.GetContextAsync();
            try
            {
                var identifier =
                    await _credentialsService.ProcessCredentialRequestAsync(context, null, new ConnectionRecord());
                _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.ConnectionsUpdated});
            }
            finally
            {
                await NavigationService.PopModalAsync();
            }
        });

        public ICommand CancelCommand => new Command(async () => await NavigationService.PopModalAsync());

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
            if (navigationData is RequestPresentationMessage request)
            {
                _requestMessage = request;
            }

            return base.InitializeAsync(navigationData);
        }

        #endregion
    }
}
