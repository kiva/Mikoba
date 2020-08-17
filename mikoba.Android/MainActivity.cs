using System;
using Java.Lang;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using System.Collections.Generic;
using System.Linq;

namespace mikoba.Droid
{
    [Activity(Label = "mikoba", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        readonly string[] _permissionsRequired =
       {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera
        };

        private int _requestCode = -1;
        private List<string> _permissionsToBeGranted = new List<string>();

        private void CheckAndRequestRequiredPermissions()
        {
            for (int i = 0; i < _permissionsRequired.Length; i++)
                if (CheckSelfPermission(_permissionsRequired[i]) != (int)Permission.Granted)
                    _permissionsToBeGranted.Add(_permissionsRequired[i]);

            if (_permissionsToBeGranted.Any())
            {
                _requestCode = 10;
                RequestPermissions(_permissionsRequired.ToArray(), _requestCode);
            }
            else
                System.Diagnostics.Debug.WriteLine("Device already has all the required permissions");
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            JavaSystem.LoadLibrary("c++_shared");
            JavaSystem.LoadLibrary("indy");

            LoadApplication(new App());

            CheckAndRequestRequiredPermissions();
            
            Xamarin.Essentials.Platform.Init(Application);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult (requestCode, permissions, grantResults);
            if (grantResults.Length == _permissionsToBeGranted.Count)
                System.Diagnostics.Debug.WriteLine("All permissions required that werent granted, have now been granted");
            else
                System.Diagnostics.Debug.WriteLine("Some permissions requested were denied by the user");

            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}