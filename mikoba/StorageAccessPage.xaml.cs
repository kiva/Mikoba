using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Hyperledger.Indy.DidApi;
using Hyperledger.Indy.WalletApi;
using Xamarin.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;

namespace mikoba
{
    public partial class StorageAccessPage : ContentPage
    {
        private string firstWalletConfig = "{\"id\":\"my_wallet\"}";
        private string secondWalletConfig = "{\"id\":\"their_wallet\"}";
        private string firstWalletCredentials = "{\"key\":\"my_wallet_key\"}";
        private string secondWalletCredentials = "{\"key\":\"their_wallet_key\"}";

        public StorageAccessPage()
        {
            InitializeComponent();
            this.btnContinue2.Clicked += BtnContinue_Clicked;
        }

        protected override async void OnAppearing()
        {
            await PutTaskDelay();
            Device.BeginInvokeOnMainThread(() =>
            {
                this.btnContinue.IsVisible = true;
                this.lblPermissions.IsVisible = false;
            });
        }

        async Task PutTaskDelay()
        {
            await Task.Delay(1000);
            this.lblProgress.Text = "Checking Permissions";
            await this.progressBar.ProgressTo(0.30, 500, Easing.Linear);
            await Task.Delay(1000);
            this.lblProgress.Text = "Getting Storage Access";
            await this.progressBar.ProgressTo(0.50, 500, Easing.Linear);
            await Task.Delay(1000);
            this.lblProgress.Text = "Creating Wallet";
            await this.progressBar.ProgressTo(1, 500, Easing.Linear);
            await this.CreateWallet();

            Application.Current.Properties["WalletCreationDate"] = DateTime.Now;
            Console.WriteLine(Application.Current.Properties.ContainsKey("WalletCreationDate"));
            await Application.Current.SavePropertiesAsync();
            this.lblProgress.Text = "Wallet Created";
        }

        async Task CreateWallet()
        {
            await Task.Delay(1000);
            
            await WalletUtils.CreateWalletAsync(firstWalletConfig, firstWalletCredentials);
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
        }

        private void BtnContinue_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HomePage());
        }
    }
}
