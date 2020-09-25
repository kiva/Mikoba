using System;
using mikoba.UI.Pages;
using Xamarin.Forms;

namespace mikoba.Services
{
    public static class NavigationService
    {
        private static NavigationPage _navigationPage;

        public static NavigationPage CreateMainPage(Func<Page> initialPageBuilder)
        {
           _navigationPage = new NavigationPage(initialPageBuilder());
           return _navigationPage;
        }
    }
}
