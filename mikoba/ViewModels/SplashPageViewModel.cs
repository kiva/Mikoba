using System.Threading.Tasks;
using System.Windows.Input;
using mikoba.Services;
using mikoba.UI;
using mikoba.UI.Pages;
using mikoba.UI.Pages.Onboarding;
using mikoba.ViewModels.Pages;
using mikoba.ViewModels.Pages.Onboarding;
using Sentry;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public class SplashPageViewModel : KivaBaseViewModel
    {
        public SplashPageViewModel(
            INavigationService navigationService)
        : base("Splash", navigationService)
        {
            
        }
        
        public override async Task InitializeAsync(object navigationData)
        {
            await base.InitializeAsync(navigationData);
        }
        
        #region Commands
        public ICommand GetStartedCommand 
        { 
            get => new Command(async () =>
            {
                await NavigationService.NavigateToAsync<WalletOwnerInputViewModel>();
            });
        }
    
        public ICommand ClaimWalletCommand {
            get => new Command(async () =>
            {
                await NavigationService.NavigateToAsync<WalletOwnerInputViewModel>();
            });
        }
        #endregion
    }
}
