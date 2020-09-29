using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Agents.Edge;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
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
    public class AcceptConnectionInviteViewModel : KivaBaseViewModel
    {
        public AcceptConnectionInviteViewModel(
            INavigationService navigationService,
            IConnectionService connectionService,
            IMessageService messageService,
            IAgentProvider contextProvider,
            IEventAggregator eventAggregator,
            ILifetimeScope scope)
            : base("Accept Invitation", navigationService)
        {
            _connectionService = connectionService;
            _contextProvider = contextProvider;
            _messageService = messageService;
            _contextProvider = contextProvider;
            _eventAggregator = eventAggregator;
            _scope = scope;
        }

        private ConnectionInvitationMessage _invite;

        #region Services

        private readonly IConnectionService _connectionService;
        private readonly IMessageService _messageService;
        private readonly IAgentProvider _contextProvider;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILifetimeScope _scope;

        #endregion

        #region Commands

        public ICommand AcceptInviteCommand => new Command(async () =>
        {
            var context = await _contextProvider.GetContextAsync();
            try
            {
                var (msg, record) = await _connectionService.CreateRequestAsync(context, _invite);
                await _messageService.SendAsync(context.Wallet, msg, record);
                _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.ConnectionsUpdated});
                var entry = _scope.Resolve<EntryViewModel>();
                entry.Connection = _scope.Resolve<SSIConnectionViewModel>(new NamedParameter("record", record));
                entry.Setup();
                await this.NavigationService.NavigateBackAsync();
            }
            catch (Exception ex)
            {
                await this.NavigationService.NavigateBackAsync();
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

        private string _inviteContents =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua";

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
                //invite.Label = "Sierra Leone Authority";
                InviteTitle = $"Trust {invite.Label}?";
                InviterUrl = invite.ImageUrl;
                InviteContents =
                    $"{invite.Label} would like to establish a pairwise DID connection with you. This will allow secure communication between you and {invite.Label}.";
                _invite = invite;
            }

            return base.InitializeAsync(navigationData);
        }

        #endregion
    }
}
