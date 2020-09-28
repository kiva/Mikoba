using mikoba.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Pages.Onboarding
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllowCameraConfirmationPage : ContentPage
    {
        public AllowCameraConfirmationPage()
        {
            InitializeComponent();
            BindingContext = new AllowCameraConfirmationViewModel(Navigation);
        }
    }
}