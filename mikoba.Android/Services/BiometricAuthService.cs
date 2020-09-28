using System;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Security.Keystore;
using Android.Support.V4.App;
using Android.Support.V4.Hardware.Fingerprint;
using Android.Util;
using Android.Widget;
using mikoba.Droid.Services;
using Java.Security;
using Javax.Crypto;
using mikoba.Services;

// [assembly: Xamarin.Forms.Dependency(typeof(BiometricAuthService))]
namespace mikoba.Droid.Services
{
    // public class BiometricAuthService : IBiometricAuthenticationService
    // {
    //     Context context = Android.App.Application.Context;
    //     private KeyStore keyStore;
    //     private Cipher cipher;
    //     private string KEY_NAME = "XamarinLife";
    //     public static bool IsAuthSucess;
    //
    //     public string GetAuthenticationType()
    //     {
    //         return "";
    //     }
    //
    //     public Task<bool> AuthenticateUserIDWithTouchID()
    //     {
    //         var tcs = new TaskCompletionSource<bool>();
    //
    //         KeyguardManager keyguardManager = (KeyguardManager) context.GetSystemService(Context.KeyguardService);
    //         FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(context);
    //
    //         if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
    //         {
    //             if (ActivityCompat.CheckSelfPermission(context, Manifest.Permission.UseBiometric)
    //                 != (int) Android.Content.PM.Permission.Granted)
    //             {
    //                 return tcs.Task;
    //             }
    //
    //             if (!fingerprintManager.IsHardwareDetected)
    //             {
    //                 Toast.MakeText(context, "FingerPrint authentication permission not enable", ToastLength.Short).Show();
    //             }
    //             else
    //             {
    //                 if (!fingerprintManager.HasEnrolledFingerprints)
    //                 {
    //                     Toast.MakeText(context, "Register at least one fingerprint in Settings", ToastLength.Short).Show();
    //                 }
    //                 else
    //                 {
    //                     if (!keyguardManager.IsKeyguardSecure)
    //                     {
    //                         Toast.MakeText(context, "Lock screen security not enable in Settings", ToastLength.Short).Show();
    //                     }
    //                     else
    //                     {
    //                         GenKey();
    //                     }
    //                     
    //                     if (CipherInit())
    //                     {
    //                         FingerprintManagerCompat.CryptoObject cryptoObject = new FingerprintManagerCompat.CryptoObject(cipher);
    //                         BiometricHandler handler = new BiometricHandler(context);
    //                         handler.StartAuthentication(fingerprintManager, cryptoObject);
    //                     }
    //                 }
    //             }
    //         }
    //         else if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
    //         {
    //             if (ActivityCompat.CheckSelfPermission(context, Manifest.Permission.UseFingerprint) !=
    //                 (int) Android.Content.PM.Permission.Granted)
    //             {
    //                 return tcs.Task;
    //             }
    //
    //             if (!fingerprintManager.IsHardwareDetected)
    //             {
    //                 Toast.MakeText(context, "FingerPrint authentication permission not enable", ToastLength.Short).Show();
    //             }
    //             else
    //             {
    //                 
    //             }
    //         }
    //     }
    // }
}