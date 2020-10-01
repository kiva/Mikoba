using System.Threading.Tasks;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Features.IssueCredential;
using Hyperledger.Aries.Features.PresentProof;
using mikoba.ViewModels.Pages;
using Xamarin.Forms;

namespace mikoba.Services
{
    public class ActionDispatcher : IActionDispatcher
    {
        public ActionDispatcher(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
 
        #region Services
        public INavigationService _navigationService;
        #endregion
        
        public async Task DispatchMessage(AgentMessage message)
        {
            if (message is ConnectionInvitationMessage inviteMessage)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await _navigationService.NavigateToAsync<AcceptConnectionInviteViewModel>(
                        inviteMessage, NavigationType.Modal);
                });
            }
            else if (message is CredentialOfferMessage credentialOffer)
            { 
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await _navigationService.NavigateToAsync<CredentialOfferPageViewModel>(
                        credentialOffer, NavigationType.Modal);
                });
            }
            else if (message is RequestPresentationMessage credentialRequest)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await _navigationService.NavigateToAsync<CredentialRequestPageViewModel>(
                        credentialRequest, NavigationType.Modal);
                });
            }
        }
    }
}
