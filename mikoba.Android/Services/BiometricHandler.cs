using System;
using Android.Content;
using Android.Hardware.Fingerprints;
using Android.Support.V4.Hardware.Fingerprint;
using Android.Support.V4.OS;
using Android.Widget;
using Xamarin.Forms;

namespace mikoba.Droid.Services
{
    // public class BiometricHandler : FingerprintManagerCompat.AuthenticationCallback
    // {
    //     private Context mainActivity;
    //
    //     public bool AuthResult;
    //     
    //     public BiometricHandler(Context mainActivity)
    //     {
    //         this.mainActivity = mainActivity;
    //     }
    //     
    //     internal void StartAuthentication(FingerprintManagerCompat fingerprintManager, FingerprintManagerCompat.CryptoObject cryptoObject)
    //     {
    //         CancellationSignal cancellationSignal = new CancellationSignal();
    //         fingerprintManager.Authenticate(cryptoObject,0, cancellationSignal, this, null);
    //     }
    //     
    //     public override void OnAuthenticationFailed()
    //     {
    //         BiometricAuthService.IsAuthSucess = false;
    //         Toast.MakeText(mainActivity, "Fingerprint Authentication failed!", ToastLength.Long).Show();
    //         MessagingCenter.Send<string>("Auth", "Fail");
    //     }
    //     
    //     public override void OnAuthenticationSucceeded(FingerprintManagerCompat.AuthenticationResult result)
    //     {
    //         BiometricAuthService.IsAuthSucess = true;
    //         Toast.MakeText(mainActivity, "Fingerprint Authentication Success", ToastLength.Long).Show();
    //         MessagingCenter.Send<string>("Auth", "Success");
    //     }
    // }
}