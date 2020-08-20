using System;
using mikoba.UI;
using Sentry;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
