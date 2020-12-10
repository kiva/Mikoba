using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using mikoba.Services;
using ReactiveUI;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public abstract class KivaBaseViewModel : ReactiveObject, IBaseViewModel
    {
        public KivaBaseViewModel(string title, INavigationService navigationService)
        {
            this.NavigationService = navigationService;
        }

        protected KivaBaseViewModel()
        {
            
        }
        
        #region Services
        protected readonly INavigationService NavigationService;
        #endregion
        
        #region Commands
        public ICommand DestroyWalletCommand
        {
            get => new Command(async ()=>
            {
                Application.Current.Properties.Clear();
                await Application.Current.SavePropertiesAsync();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            });
        }

        public Command GoBackCommand
        {
            get => new Command( () =>
            {
                if (this.NavigationService != null)
                {
                    // this.NavigationService.PopAsync();
                }
            });
        }

        public Command GoToSettingsCommand { 
            get => new Command( () =>
            {
                if (this.NavigationService == null)
                {
                    return;
                }
                // await Services.NavigationService.PushAsync(new SettingsPage());
            });
        }
        #endregion

        #region Image Helpers
        public Assembly SvgAssembly { get; } = typeof(App).GetTypeInfo().Assembly;

        public string KivaLogo { get; } = "mikoba.Images.kiva.svg";

        public string Wave { get; } = "mikoba.Images.wave.svg";

        public string Bsl { get; } = "mikoba.Images.bsl.svg";

        public string Leaf { get; } = "mikoba.Images.leaf.svg";

        public string Orange { get; } = "mikoba.Images.orange.svg";

        public string Dots { get; } = "mikoba.Images.dots.svg";

        public string Pink { get; } = "mikoba.Images.pink.svg";

        public string LocationPin { get; } = "mikoba.Images.locationpin.svg";

        public string QrCodeScan { get; } = "mikoba.Images.qrcodescan.svg";

        public string QrCodeScan2 { get; } = "mikoba.Images.qrcodescan2.svg";
        public string RightCaret { get; } = "mikoba.Images.rightcaret.svg";

        public string RightCaretYellow { get; } = "mikoba.Images.rightcaretyellow.svg";

        public string LeftArrowYellow { get; } = "mikoba.Images.leftarrow_yellow.svg";
        public string Secure { get; } = "mikoba.Images.secure.svg";

        public string Clock { get; } = "mikoba.Images.clock.svg";

        public string Selfie { get; } = "mikoba.Images.selfie.svg";

        public string KivaLogoBlue { get; } = "mikoba.Images.kivalogoblue.svg";

        public string Gear { get; } = "mikoba.Images.gear.svg";

        public string CornerCircles { get; } = "mikoba.Images.cornercircles.svg";

        public string NoImage { get; } = "mikoba.Images.noImage.svg";

        public string CornerCircles_Left { get; } = "mikoba.Images.cornercircles_left.svg";

        public string CameraIcon { get; } = "mikoba.Images.camera_icon.svg";

        public string BellIcon { get; } = "mikoba.Images.bell.svg";

        public string Anonymous { get; } = "mikoba.Images.anonymousprofilepic.svg";

        public string FingerprintIcon { get; } = "mikoba.Images.fingerprint_icon.svg";
        
        #endregion

        #region UI Properties
        public string Name { get; set; }
        public string Title { get; set; }
        public bool IsBusy { get; set; }
        #endregion
        
        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
