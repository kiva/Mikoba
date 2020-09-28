using System;
using System.Linq;
using System.Threading.Tasks;
using mikoba.Services;
using mikoba.UI.Pages.Wallet;
using mikoba.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage, IRootView
    {
        public SplashPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            this.AppLogo.IsVisible = false;
            this.gridOptions.IsVisible = true;
        }
    }
}
