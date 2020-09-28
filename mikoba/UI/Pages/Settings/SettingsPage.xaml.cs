using mikoba.UI.ViewModels;
using Xamarin.Forms;

namespace mikoba.UI.Pages.Settings
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = SettingsPageViewModel.Instance;
            SettingsPageViewModel.Instance.NavigationService = this.Navigation;
        }
    }
}
