using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using mikoba.ViewModels;
using Xamarin.Forms;

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