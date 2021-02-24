using System;
using Java.Lang;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using FFImageLoading.Forms.Platform;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Plugin.CurrentActivity;
using Plugin.Fingerprint;
using Sentry;
using SVG.Forms.Plugin.Droid;

namespace mikoba.Droid
{
    [Activity(Label = "Kiva Protocol Wallet", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            
            JavaSystem.LoadLibrary("c++_shared");
            JavaSystem.LoadLibrary("indy");

            //Telemetry
            SentrySdk.Init("https://29fb995fc23549159102c71041f25617@o7540.ingest.sentry.io/5320493");

            //Xamarin Plugin Support
            CachedImageRenderer.Init(false);
            SvgImageRenderer.Init();
            Xamarin.Essentials.Platform.Init(Application);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            ZXing.Mobile.MobileBarcodeScanner.Initialize(Application);
            
            CheckAndRequestRequiredPermissions();
            
            // Initializing User Dialogs
            // Android requires that we set content root.
            var host = App
                .BuildHost(typeof(PlatformModule).Assembly)
                .UseContentRoot(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)).Build();

            
            LoadApplication(host.Services.GetRequiredService<App>());
            CrossFingerprint.SetCurrentActivityResolver(() => CrossCurrentActivity.Current.Activity);   
        }
        
        private readonly string[] _permissionsRequired =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera,
        };

        private int _requestCode = -1;
        private readonly List<string> _permissionsToBeGranted = new List<string>();

        private void CheckAndRequestRequiredPermissions()
        {
            for (var i = 0; i < _permissionsRequired.Length; i++)
            {
                if (CheckSelfPermission(_permissionsRequired[i]) != (int) Permission.Granted)
                {
                    _permissionsToBeGranted.Add(_permissionsRequired[i]);
                }
            }
            if (_permissionsToBeGranted.Any())
            {
                _requestCode = 10;
                RequestPermissions(_permissionsRequired.ToArray(), _requestCode);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Device already has all the required permissions");
            }
        }

        private class PlatformModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                base.Load(builder);
                builder.RegisterModule(new KernelModule());
            }
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            var diagnosticTest = "All permissions required that weren't granted, have now been granted";
            if (grantResults.Length != _permissionsToBeGranted.Count)
            {
                diagnosticTest = "Some permissions requested were denied by the user";
            }
            System.Diagnostics.Debug.WriteLine(diagnosticTest);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
    }
}
