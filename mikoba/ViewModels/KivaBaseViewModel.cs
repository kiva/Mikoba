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
        public Command GoBackCommand { get; set; }
        
        public INavigation NavigationService { get;  set; }

        public KivaBaseViewModel()
        {
            this.DestroyWalletCommand = new Command(async () =>
            {
                Application.Current.Properties.Clear();
                await Application.Current.SavePropertiesAsync();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            });
            GoBackCommand = new Command(async () =>
            {
                if (this.NavigationService != null)
                {
                    this.NavigationService.PopAsync();
                }
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
        
        public string Bsl
        {
            get { return "mikoba.Images.bsl.svg"; }
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
        
        public string QrCodeScan2
        {
            get { return "mikoba.Images.qrcodescan2.svg"; }
        }
        
        public string RightCaret
        {
            get { return "mikoba.Images.rightcaret.svg"; }
        }
        
        public string RightCaretYellow
        {
            get { return "mikoba.Images.rightcaretyellow.svg"; }
        }

        public string LeftArrowYellow
        {
            get { return "mikoba.Images.leftarrow_yellow.svg"; }
        }
        
        public string Secure
        {
            get { return "mikoba.Images.secure.svg"; }
        }
        
        public string Clock
        {
            get { return "mikoba.Images.clock.svg"; }
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

        public string CornerCircles
        {
            get { return "mikoba.Images.cornercircles.svg";  }
        }

        public string CornerCircles_Left
        {
            get { return "mikoba.Images.cornercircles_left.svg"; }
        }

        public string CameraIcon
        {
            get { return "mikoba.Images.camera_icon.svg"; }
        }

        public string BellIcon
        {
            get { return "mikoba.Images.bell.svg"; }
        }

        public string Anonymous
        {
            get { return "mikoba.Images.anonymousprofilepic.svg";  }
        }
    }
}
