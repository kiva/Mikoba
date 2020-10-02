using mikoba.ViewModels;
using Xamarin.Forms;

namespace mikoba.UI.Components
{
    public partial class CredentialOfferCard : ContentView
    {
        public CredentialOfferCard()
        {
            InitializeComponent();
            this.BindingContext = CredentialOfferCardViewModel.Instance;
            
        }
    }
}
