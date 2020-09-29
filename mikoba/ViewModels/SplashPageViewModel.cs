using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.Services;
using mikoba.UI;
using mikoba.UI.Pages;
using mikoba.UI.Pages.Onboarding;
using mikoba.ViewModels.Pages;
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
            // if (Preferences.Get(AppConstant.LocalWalletProvisioned, false))
            // {
            //     await NavigationService.NavigateToAsync<WalletPageViewModel>();
            // }
            await base.InitializeAsync(navigationData);
        }
        
        #region Commands
        public ICommand GetStartedCommand 
        { 
            get => new Command(async () =>
            {
                await WalletService.ProvisionWallet();
                Preferences.Set(AppConstant.LocalWalletProvisioned, true);
                await NavigationService.NavigateToAsync<WalletPageViewModel>();
            });
        }
    
        public ICommand ClaimWalletCommand {
            get => new Command(async () =>
            {
                //TODO
            });
        }
        #endregion
    }
}
