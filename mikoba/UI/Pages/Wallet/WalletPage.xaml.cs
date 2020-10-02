using mikoba.Extensions;
using mikoba.Services;
using Xamarin.Forms;

namespace mikoba.UI.Pages.Wallet
{
    public partial class WalletPage : ContentPage, IRootView
    {
        public WalletPage()
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
