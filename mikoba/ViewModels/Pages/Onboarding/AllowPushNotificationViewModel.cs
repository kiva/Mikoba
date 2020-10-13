using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.Services;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages.Onboarding
{
    public class AllowPushNotificationViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public AllowPushNotificationViewModel(INavigationService navigationService)
        : base("Allow Push Notifications", navigationService)
        {
            GoBack = new Command(async () =>
            {
                await NavigationService.NavigateBackAsync();
            });
            
            CheckNotificationPermissions = new Command(async () =>
            {
                // var status = await Permissions.RequestAsync<Permissions.Reminders>();
                // if (status == PermissionStatus.Granted)
                // {
                AdvancePage();
                // }
            });
            
            SkipStep = new Command(async () =>
            {
                AdvancePage();
            });
        }
        
        
        #region Commands
        public ICommand GoBack { get; set; }
        
        public ICommand CheckNotificationPermissions { get; set; }
        
        public ICommand SkipStep { get; set; }

        #endregion

        public async void AdvancePage()
        {
            await NavigationService.NavigateToAsync<WalletCreationViewModel>();
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}