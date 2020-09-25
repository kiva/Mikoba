using System.ComponentModel;
using mikoba.Extensions;
using mikoba.ViewModels;
using Xamarin.Forms;

namespace mikoba.UI.Pages.Wallet
{
    [DesignTimeVisible(false)]
    public partial class FirstActionsPage : ContentPage
    {
        public FirstActionsPage()
        {
            InitializeComponent();
            WalletFirstActionsPageViewModel.Instance.NavigationService = this.Navigation;
            this.BindingContext = WalletFirstActionsPageViewModel.Instance;
        }

        protected override void OnAppearing()
        {
            this.CorrectSafeMargin();
        }
    }
    
    
}
