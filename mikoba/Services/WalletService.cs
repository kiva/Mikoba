using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Autofac;
using Hyperledger.Aries.Agents.Edge;
using Hyperledger.Indy.WalletApi;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.Services
{
    public static class WalletService
    {
        public static string BuildJsonObject(string fieldName, string value)
        {
            return "{\"" + fieldName + "\":\"" + value + "\"}";
        }
    
        public static async Task ProvisionWallet()
        {
            var provisioningService = App.Container.Resolve<IEdgeProvisioningService>();
            await provisioningService.ProvisionAsync();
            Preferences.Set(AppConstant.LocalWalletProvisioned, true);
        }

        public static async Task CreateWallet()
        {
            var id = Guid.NewGuid();
            var config = WalletService.BuildJsonObject("id", id.ToString());
            var creds = WalletService.BuildJsonObject("key", "my_wallet_key");
            await Wallet.CreateWalletAsync(config, creds);
            Application.Current.Properties["WalletCreationDate"] = DateTime.Now;
            Application.Current.Properties["WalletId"] = id.ToString();
            await Application.Current.SavePropertiesAsync();
        }
        
        public static async Task<Wallet> OpenWallet()
        {
            var id = Application.Current.Properties["WalletId"];
            var config = WalletService.BuildJsonObject("id", id.ToString());
            var creds = WalletService.BuildJsonObject("key", "my_wallet_key");
            return await Wallet.OpenWalletAsync(config, creds);
        }
    }
}
