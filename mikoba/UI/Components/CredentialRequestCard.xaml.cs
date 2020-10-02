using mikoba.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CredentialRequestCard : ContentView
    {
        public CredentialRequestCard()
        {
            InitializeComponent();
            this.BindingContext = CredentialRequestCardViewModel.Instance;
        }
    }
}