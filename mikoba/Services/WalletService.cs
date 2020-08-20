using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Hyperledger.Indy.WalletApi;
using Xamarin.Forms;

namespace mikoba.Services
{
    public class WalletService
    {
        public static string BuildJsonObject(string fieldName, string value)
        {
            return "{\"" + fieldName + "\":\"" + value + "\"}";
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

/*
 *             await WalletUtils.CreateWalletAsync(firstWalletConfig, firstWalletCredentials);
            using (var firstWallet = await Wallet.OpenWalletAsync(firstWalletConfig, firstWalletCredentials))
            {
                var myDid = await Did.CreateAndStoreMyDidAsync(firstWallet, "{}");

                var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                var exportConfig = JsonConvert.SerializeObject(new
                {
                    path = path,
                    key = Guid.NewGuid().ToString()
                });

                await firstWallet.ExportAsync(exportConfig);
                
                await Wallet.ImportAsync(secondWalletConfig, secondWalletCredentials, exportConfig);
                
                using (var secondWallet = await Wallet.OpenWalletAsync(secondWalletConfig, secondWalletCredentials))
                {
                    var myKey = await Did.KeyForLocalDidAsync(secondWallet, myDid.Did);
                    Debug.Assert(myKey == myDid.VerKey);
                    await secondWallet.CloseAsync();
                }
                await firstWallet.CloseAsync();
                File.Delete(path);
                Console.WriteLine("Wallet Creation OK", Color.Green);
            }
 */
