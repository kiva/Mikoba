using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages.Onboarding
{
    public class AllowCameraConfirmationViewModel : MikobaBaseViewModel, INotifyPropertyChanged
    {
        public ICommand GoBack { get; set; }

        public ICommand CheckCameraPermissions { get; set; }
        
        public ICommand SkipStep { get; set; }
        
        public AllowCameraConfirmationViewModel(INavigationService navigationService)
            : base("Allow Camera", navigationService)
        {
            GoBack = new Command(async () =>
            {
                await NavigationService.NavigateBackAsync();
            });
            
            CheckCameraPermissions = new Command(async () =>
            {
                var status = await Permissions.RequestAsync<Permissions.Camera>();
                if (status == PermissionStatus.Granted)
                {
                    AdvancePage();
                }
            });
            
            SkipStep = new Command( () =>
            {
                AdvancePage();
            });
        }

        public async void AdvancePage()
        {
            await NavigationService.NavigateToAsync<AllowPushNotificationViewModel>();
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
