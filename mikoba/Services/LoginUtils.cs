using System;
using System.Linq;
using mikoba.UI.Pages.Wallet;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.Services
{
    public class LoginUtils
    {
        public INavigation Navigation { get; set; }
        
        public LoginUtils(INavigation navigation)
        {
            Navigation = navigation;
        }
        
        public async void YouShallPass()
        {
            if (Preferences.Get(AppConstant.LocalWalletFirstView, false))
            {
                Console.WriteLine("Navigating to Wallet First Actions Sequence");
                var page = Navigation.NavigationStack.Last();
                await Navigation.PushAsync(new FirstActionsPage());
                Navigation.RemovePage(page);
            }
            else
            {
                Console.WriteLine("Navigating to Wallet Page");
                var page = Navigation.NavigationStack.Last();
                await Navigation.PushAsync(new WalletPage());
                Navigation.RemovePage(page);
            }
        }
    }
}