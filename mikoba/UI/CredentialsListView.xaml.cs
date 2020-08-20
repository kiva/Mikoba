using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace mikoba.UI
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class CredentialsListView : ContentView
    {
        public CredentialsListView()
        {
            InitializeComponent();
            BindingContext = new CredentialsListViewModel();
        }

    }
}