using mikoba.Extensions;
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
