﻿using FFImageLoading.Forms.Platform;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using Sentry;
using SVG.Forms.Plugin.iOS;
using UIKit;

namespace mikoba.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private App _application;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
#if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
#endif
            global::Xamarin.Forms.Forms.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            SentrySdk.Init("https://1bcab2b0bbf64a7eb7af005293f5ef2d@o7540.ingest.sentry.io/5320492");
            
            //Image Plugin Support
            CachedImageRenderer.Init();
            SvgImageRenderer.Init();
            
                var host = App.BuildHost(typeof(KernelModule).Assembly).Build();
                _application = host.Services.GetRequiredService<App>();
                LoadApplication(new App());
                
            return base.FinishedLaunching(app, options);
        }
    }
}
