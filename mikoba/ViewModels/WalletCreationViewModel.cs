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
using Sentry;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels
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
        
        #region Lifecyle
        
        public override async Task InitializeAsync(object navigationData)
        {
            await _edgeProvisioningService.ProvisionAsync();
            Preferences.Set(AppConstant.LocalWalletProvisioned, true);
            await Task.Delay(2000);
            await NavigationService.NavigateToAsync<WalletPageViewModel>();
        }
        
        #endregion

    }
}
