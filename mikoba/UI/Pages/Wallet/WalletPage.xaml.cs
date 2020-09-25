using System;
using mikoba.Extensions;
using mikoba.UI.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.UI.Pages.Wallet
{
    public partial class WalletPage : ContentPage
    {
        public WalletPage()
        {
            InitializeComponent();
            BindingContext = WalletPageViewModel.Instance;
            WalletPageViewModel.Instance.NavigationService = this.Navigation;
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            // WalletPageViewModel.Instance.SettingsCommand.Execute(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.CorrectSafeMargin();
            Preferences.Set(AppConstant.LocalWalletFirstView, false);
        }
    }
}
