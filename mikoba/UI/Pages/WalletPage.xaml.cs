using System;
using System.ComponentModel;
using mikoba.UI.ViewModels;
using mikoba.ViewModels;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var current = this.Padding;
            current.Bottom = 0;
            this.Padding = current;
        }
    }
}
