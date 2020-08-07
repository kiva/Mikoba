using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace mikoba
{
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            this.btnGetStarted.Clicked += BtnGetStarted_Clicked;
            this.btnClaimExistingWallet.Clicked += BtnClaimExistingWallet_Clicked;
        }

        private async void BtnClaimExistingWallet_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "Not Implemented Yet!", "OK");
        }

        private void BtnGetStarted_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StorageAccessPage());
        }
    }
}
