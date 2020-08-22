using System.ComponentModel;
using Xamarin.Forms;
using ZXing;

namespace mikoba.UI.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ScanQrCodePage : ContentPage
    {

        public void Handle_OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Scanned result", result.Text, "OK");
            });
        }

        protected override void OnAppearing()
        {
            ScanView.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            ScanView.IsScanning = false;
        }
    }
}