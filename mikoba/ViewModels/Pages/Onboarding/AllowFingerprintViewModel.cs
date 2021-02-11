using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.Services;
using mikoba.ViewModels.Pages.Login;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages.Onboarding
{
    public class AllowFingerprintViewModel : MikobaBaseViewModel, INotifyPropertyChanged
    {
        public ICommand GoBack { get; set; }
        
        public ICommand SkipStep { get; set; }
        
        public ICommand AllowFingerprints { get; set; }
        
        public AllowFingerprintViewModel(INavigationService navigationService) : base("Fingerprint Permission", navigationService)
        {
            GoBack = new Command(async () =>
            {
                await NavigationService.NavigateBackAsync();
            });
            
            AllowFingerprints = new Command(async () =>
            {
                Preferences.Set(AppConstant.AllowFingerprint, true);
                await  NavigationService.NavigateToAsync<AllowCameraConfirmationViewModel>();
            });
            
            SkipStep = new Command(async () =>
            {
                await  NavigationService.NavigateToAsync<AllowCameraConfirmationViewModel>();
            });
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
