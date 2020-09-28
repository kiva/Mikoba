using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.UI.Pages;
using mikoba.UI.Pages.Onboarding;
using Polly;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public class AllowCameraConfirmationViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public ICommand GoBack { get; set; }
        
        private INavigation NavigationService { get; set; }
        
        public ICommand CheckCameraPermissions { get; set; }
        
        public ICommand SkipStep { get; set; }
        
        public AllowCameraConfirmationViewModel(INavigation navigationService)
        {
            NavigationService = navigationService;
            GoBack = new Command(async () =>
            {
                await NavigationService.PopAsync(true);
            });
            
            CheckCameraPermissions = new Command(async () =>
            {
                var status = await Permissions.RequestAsync<Permissions.Camera>();
                if (status == PermissionStatus.Granted)
                {
                    AdvancePage();
                }
            });
            
            SkipStep = new Command(async () =>
            {
                AdvancePage();
            });
        }

        public void AdvancePage()
        {
            NavigationService.PushAsync(new AllowPushNotificationPage(), true);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}