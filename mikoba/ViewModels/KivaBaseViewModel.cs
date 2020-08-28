using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.UI;
using mikoba.UI.Pages;
using Sentry;
using Xamarin.Forms;


namespace mikoba.ViewModels
{
    public class KivaBaseViewModel
    {
        public ICommand DestroyWalletCommand { get; set; }

        public KivaBaseViewModel()
        {
            this.DestroyWalletCommand = new Command(async () =>
            {
                Application.Current.Properties.Clear();
                await Application.Current.SavePropertiesAsync();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            });
        }
        
        public Assembly SvgAssembly
        {
            get { return typeof(App).GetTypeInfo().Assembly; }
        }

        public string KivaLogo
        {
            get { return "mikoba.Images.kiva.svg"; }
        }

        public string Wave
        {
            get { return "mikoba.Images.wave.svg"; }
        }
        
        public string Leaf
        {
            get { return "mikoba.Images.leaf.svg"; }
        }

        public string Orange
        {
            get { return "mikoba.Images.orange.svg"; }
        }

        public string Dots
        {
            get { return "mikoba.Images.dots.svg"; }
        }
        
        public string Pink
        {
            get { return "mikoba.Images.pink.svg"; }
        }

        public string LocationPin
        {
            get { return "mikoba.Images.locationpin.svg"; }
        }
        
        public string QrCodeScan
        {
            get { return "mikoba.Images.qrcodescan.svg"; }
        }
        
        public string RightCaret
        {
            get { return "mikoba.Images.rightcaret.svg"; }
        }
        
        public string Secure
        {
            get { return "mikoba.Images.secure.svg"; }
        }
        
        public string Selfie
        {
            get { return "mikoba.Images.selfie.svg"; }
        }
        
        public string KivaLogoBlue
        {
            get { return "mikoba.Images.kivalogoblue.svg"; }
        }
        
        public string Gear
        {
            get { return "mikoba.Images.gear.svg"; }
        }
    }
}
