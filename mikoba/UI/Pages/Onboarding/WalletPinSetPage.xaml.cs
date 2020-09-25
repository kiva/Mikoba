using mikoba.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Pages.Onboarding
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalletPinSetPage : ContentPage
    {
        public WalletPinSetPage(string name)
        {
            InitializeComponent();
            var model = new WalletPinSetViewModel(Navigation);
            model.SetFirstName(name);
            BindingContext = model;
        }
    }
}