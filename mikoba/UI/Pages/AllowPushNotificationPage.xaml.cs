using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mikoba.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllowPushNotificationPage : ContentPage
    {
        public AllowPushNotificationPage()
        {
            InitializeComponent();
            BindingContext = new AllowPushNotificationViewModel(Navigation);
        }
    }
}