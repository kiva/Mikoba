using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mikoba.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PINLoginPage : ContentPage
    {
        public PINLoginPage()
        {
            InitializeComponent();
            InitializeComponent();
            var model = new PINLoginViewModel(Navigation);
            model.SetPIN(Application.Current.Properties["WalletPIN"] as string);
            BindingContext = model;
        }
    }
}