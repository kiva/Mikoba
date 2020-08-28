using System;
using System.ComponentModel;
using mikoba.UI.Pages;
using mikoba.UI.ViewModels;
using Xamarin.Forms;

namespace mikoba.UI
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class CreateConnectionPage : ContentPage
    {
        public CreateConnectionPage()
        {
            InitializeComponent();
            BindingContext = new CreateConnectionViewModel();
        }

        async void OnScanQrCodePageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QrScanPage());
        }
    }
}
