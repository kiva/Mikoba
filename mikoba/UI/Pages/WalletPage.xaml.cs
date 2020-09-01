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
        }
    }
}
