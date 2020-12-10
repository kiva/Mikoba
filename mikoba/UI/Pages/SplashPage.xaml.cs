using mikoba.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage, IRootView
    {
        public SplashPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.AppLogo.IsVisible = false;
            this.gridOptions.IsVisible = true;
        }
    }
}
