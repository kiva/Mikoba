using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
            if (Application.Current.Properties.ContainsKey("WalletCreationDate"))
            {
                MainPage = new NavigationPage(new HomePage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}
