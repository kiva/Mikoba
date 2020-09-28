using mikoba.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Pages.Onboarding
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalletPinConfirmationPage : ContentPage
    {
        public WalletPinConfirmationPage(string PIN)
        {
            InitializeComponent();
            var model = new WalletPinConfirmViewModel(Navigation);
            model.SetPIN(PIN);
            BindingContext = model;
        }
    }
}