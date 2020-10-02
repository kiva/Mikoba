using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using Hyperledger.Aries.Agents.Edge;
using Hyperledger.Aries.Configuration;
using mikoba.Annotations;
using mikoba.Services;
using mikoba.UI;
using mikoba.ViewModels.Pages;
using ReactiveUI;
using Sentry;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages.Onboarding
{
    public sealed class WalletCreationViewModel : KivaBaseViewModel
    {

        public WalletCreationViewModel(
            INavigationService navigationService,
            IEdgeProvisioningService edgeProvisioningService
        )
        : base("Wallet Creation", navigationService)
        {
            _edgeProvisioningService = edgeProvisioningService;
        }

        private IEdgeProvisioningService _edgeProvisioningService;
        
        #region UI Properties
        
        private string _progressInfo;
        public string ProgressInfo
        {
            get => _progressInfo;
            set => this.RaiseAndSetIfChanged(ref _progressInfo, value);
        }
        
        private double _progress;
        public double Progress
        {
            get => _progress;
            set => this.RaiseAndSetIfChanged(ref _progress, value);
        }
        
        #endregion
        
        #region Lifecyle
        
        public override async Task InitializeAsync(object navigationData)
        {
            await _edgeProvisioningService.ProvisionAsync();
            await Task.Delay(100);
            ProgressInfo = "Checking Permissions";
            Progress = 0.30;
            await Task.Delay(100);
            ProgressInfo = "Getting Storage Access";
            Progress = 0.50;
            await Task.Delay(100);
            ProgressInfo = "Creating Wallet";
            Progress = 1;
            await WalletService.ProvisionWallet();
            ProgressInfo = "Wallet Created";
            Preferences.Set(AppConstant.LocalWalletProvisioned, true);
            await Task.Delay(2000);
            await NavigationService.NavigateToAsync<WalletPageViewModel>();
        }
        
        #endregion

    }
}
