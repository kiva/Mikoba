using System;
using System.ComponentModel;
using mikoba.ViewModels;
using Xamarin.Forms;

namespace mikoba.UI
{
    
    public partial class CredentialsListView : ContentPage
    {
        public CredentialsListView()
        {
            InitializeComponent();
            BindingContext = CredentialsListViewModel.Instance;
        }
    }
}
