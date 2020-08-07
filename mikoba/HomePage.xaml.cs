using System;
using System.Collections.Generic;
using System.Globalization;
using mikoba.UI;
using Xamarin.Forms;

namespace mikoba
{
    public partial class HomePage : TabbedPage
    {
        public HomePage()
        {
            InitializeComponent();
            var data = Application.Current.Properties["WalletCreationDate"];
            var strDate = ((DateTime)data).ToString("D", CultureInfo.CurrentCulture);
            this.lblInfo.Text = String.Format("Wallet Created on {0}", strDate);
            this.btnDestroyWallet.Clicked += BtnDestroyWallet_Clicked;
            this.BindingContext = new MenuPageViewModel(this.Navigation);
        }

        private async void BtnDestroyWallet_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties.Clear();
            await Application.Current.SavePropertiesAsync();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
