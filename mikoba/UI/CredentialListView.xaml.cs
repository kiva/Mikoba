using System;
using System.ComponentModel;
using mikoba.ViewModels;
using Xamarin.Forms;

namespace mikoba.UI
{
    
    public partial class CredentialListView : ContentPage
    {
        public CredentialListView()
        {
            InitializeComponent();
            BindingContext = CredentialListViewModel.Instance;
        }
    }
}
