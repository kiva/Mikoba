using System;
using mikoba.UI.Pages;
using Sentry;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
[assembly: ExportFont("KivaPostGrot-Bold.ttf")]
[assembly: ExportFont("KivaPostGrot-BoldItalic.ttf")]
[assembly: ExportFont("KivaPostGrot-Book.ttf")]
[assembly: ExportFont("KivaPostGrot-BookItalic.ttf")]
[assembly: ExportFont("KivaPostGrot-Light.ttf")]
[assembly: ExportFont("KivaPostGrot-LightItalic.ttf")]
[assembly: ExportFont("KivaPostGrot-Medium.ttf")]
[assembly: ExportFont("KivaPostGrot-MediumItalic.ttf")]

namespace mikoba
{
    public partial class App : Application
    {
        private IDisposable sentry;
        public App()
        {
            InitializeComponent();
            sentry = SentrySdk.Init("https://62def4f857964cc88353443f784b5e70@o7540.ingest.sentry.io/5382430");
        }

        protected override void OnStart()
        {
            Application.Current.Properties.Remove("WalletInitialized");
            Application.Current.Properties.Remove("WalletCreationDate");
            SentrySdk.CaptureEvent(new SentryEvent(){Message = "App Starting"});
            MainPage = new NavigationPage(new SplashPage());
        }

        protected override void OnSleep()
        {
            SentrySdk.CaptureEvent(new SentryEvent(){Message = "App Sleeping"});
        }

        protected override void OnResume()
        {
            SentrySdk.CaptureEvent(new SentryEvent(){Message = "App Resuming"});
        }
    }
}
