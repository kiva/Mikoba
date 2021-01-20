using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using mikoba.Extensions;
using mikoba.ViewModels.Components;
using mikoba.ViewModels.SSI;
using ReactiveUI;
using Sentry;
using Sentry.Protocol;
using West.Extensions.Xamarin;
using Xamarin.Forms;
using INavigationService = mikoba.Services.INavigationService;

namespace mikoba.ViewModels.Pages
{
    public class AcceptConnectionInviteViewModel : KivaBaseViewModel
    {
        public AcceptConnectionInviteViewModel(
            INavigationService navigationService,
            IConnectionService connectionService,
            IMessageService messageService,
            IAgentProvider contextProvider,
            IEventAggregator eventAggregator,
            IDialogService dialogService,
            ILifetimeScope scope)
            : base("Accept Invitation", navigationService)
        {
            _connectionService = connectionService;
            _contextProvider = contextProvider;
            _messageService = messageService;
            _contextProvider = contextProvider;
            _eventAggregator = eventAggregator;
            _scope = scope;
            _dialogService = dialogService;
        }

        private ConnectionInvitationMessage _invite;

        #region Services

        private readonly IConnectionService _connectionService;
        private readonly IMessageService _messageService;
        private readonly IAgentProvider _contextProvider;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILifetimeScope _scope;
        private readonly IDialogService _dialogService;
        
        

        #endregion

        #region Commands

        public ICommand AcceptInviteCommand => new Command(async () =>
        {
            var context = await _contextProvider.GetContextAsync();
            try
            {
                SentrySdk.CaptureEvent(new SentryEvent()
                {
                    Message = "Click Accept Connection",
                    Level = SentryLevel.Info
                });
                Analytics.TrackEvent("Click Accept Connection");
                
                var (msg, record) = await _connectionService.CreateRequestAsync(context, _invite);
                msg.Label = _invite.Label;
                msg.ImageUrl = _invite.ImageUrl;
                await _messageService.SendAsync(context.Wallet, msg, record);
                _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.ConnectionsUpdated});
                _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.ConnectionCreated});

                var entry = _scope.Resolve<EntryViewModel>();
                entry.Connection = _scope.Resolve<SSIConnectionViewModel>(new NamedParameter("record", record));
                entry.Setup();
                
                SentrySdk.CaptureEvent(new SentryEvent()
                {
                    Message = "Accepted Connection",
                    Level = SentryLevel.Info
                });
                Analytics.TrackEvent("Accepted Connection");
                
                await this.NavigationService.PopModalAsync();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                SentrySdk.CaptureException(ex);
                Console.WriteLine(ex.Message);
                await _dialogService.ShowAlertAsync("Can't accept connection", ex.Message, "OK");
                await this.NavigationService.PopModalAsync();
            }
        });

        public ICommand RejectInviteCommand => new Command(async () => await NavigationService.PopModalAsync());

        #endregion

        #region UI Properties

        private string _inviteTitle;

        public string InviteTitle
        {
            get => _inviteTitle;
            set => this.RaiseAndSetIfChanged(ref _inviteTitle, value);
        }

        private string _inviteContents = "";

        public string InviteContents
        {
            get => _inviteContents;
            set => this.RaiseAndSetIfChanged(ref _inviteContents, value);
        }

        private string _inviterUrl;

        public string InviterUrl
        {
            get => _inviterUrl;
            set => this.RaiseAndSetIfChanged(ref _inviterUrl, value);
        }

        #endregion

        #region Work

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is ConnectionInvitationMessage invite)
            {
                InviteTitle = $"Allow {invite.Label} to send you requests?";
                InviterUrl = invite.ImageUrl;
                InviteContents =
                    $"{invite.Label} will be allowed to issue you credentials or verify your existing credentials.";
                _invite = invite;
            }
            return base.InitializeAsync(navigationData);
        }

        #endregion
    }
}
