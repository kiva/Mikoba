using System;
using System.ComponentModel;
using mikoba.Extensions;
using mikoba.Services;
using mikoba.ViewModels;
using Xamarin.Forms;

namespace mikoba.UI.Pages.Wallet
{
    [DesignTimeVisible(false)]
    public partial class FirstActionsPage : ContentPage, IRootView
    {
        public FirstActionsPage()
        {
            InitializeComponent();
            FirstActionsPageViewModel.Instance.NavigationService = this.Navigation;
            this.BindingContext = FirstActionsPageViewModel.Instance;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.CorrectSafeMargin();
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            FirstActionsPageViewModel.Instance.GoToSettingsCommand.Execute(this);    
        }
    }
    
    
}
