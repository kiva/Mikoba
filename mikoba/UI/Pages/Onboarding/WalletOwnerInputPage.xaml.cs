using System.Threading.Tasks;
using mikoba.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Pages.Onboarding
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalletOwnerInputPage : ContentPage
    {
        public WalletOwnerInputPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(200);
            OwnerEntry.Focus();
        }
    }
}
