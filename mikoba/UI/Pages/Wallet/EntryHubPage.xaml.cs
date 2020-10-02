using System;
using mikoba.Extensions;
using mikoba.Services;
using mikoba.UI.ViewModels;
using mikoba.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.UI.Pages.Wallet
{
    public partial class EntryHubPage : ContentPage
    {
        public EntryHubPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.CorrectSafeMargin();
        }
    }
}
