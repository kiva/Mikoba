using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace mikoba.UI
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class CredentialsListPage : ContentPage
    {
        public CredentialsListPage()
        {
            InitializeComponent();
            BindingContext = new CredentialsListViewModel();
        }

        async void OnConnectionsPageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConnectionsListPage());
        }
    }
}
