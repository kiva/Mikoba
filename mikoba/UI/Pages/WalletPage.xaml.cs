using System;
using mikoba.UI.ViewModels;
using Xamarin.Forms;

namespace mikoba.UI.Pages
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
            WalletPageViewModel.Instance.SettingsCommand.Execute(this);
        }
    }
}
