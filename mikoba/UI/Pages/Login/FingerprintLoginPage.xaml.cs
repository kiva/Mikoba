using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mikoba.Services;
using mikoba.ViewModels.Pages.Login;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Pages.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FingerprintLoginPage : ContentPage, IRootView
    {
        public FingerprintLoginPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is FingerprintLoginViewModel context)
            {
                await Task.Delay(1000);
                context.BeginAuthentication();
            }
        }
    }
}