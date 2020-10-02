using System.Threading.Tasks;
using System.Windows.Input;
using mikoba.Services;
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
                //If you are in a hurry
                //#if Debug
                // await WalletService.ProvisionWallet();
                // Preferences.Set(AppConstant.LocalWalletProvisioned, true);
                //#endif
                await NavigationService.NavigateToAsync<WalletOwnerInputViewModel>();
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
