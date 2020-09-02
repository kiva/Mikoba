using System;
using System.ComponentModel;
using mikoba.UI.ViewModels;
using mikoba.ViewModels;
using Xamarin.Forms;

namespace mikoba.UI.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ConnectionDetailsPage : ContentPage
    {
        public ConnectionDetailsPage()
        {
            InitializeComponent();
            BindingContext = new ConnectionDetailsViewModel();
        }

        async void OnCredentialOfferReviewPageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CredentialOfferReviewPage());
        }
    }
}
