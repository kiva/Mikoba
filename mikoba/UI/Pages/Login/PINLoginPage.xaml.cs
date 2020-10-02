using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mikoba.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mikoba.UI.Pages.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PINLoginPage : ContentPage, IRootView
    {
        public PINLoginPage()
        {
            InitializeComponent();
        }
    }
}