using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.UI.Pages;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public class AllowPushNotificationViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public ICommand GoBack { get; set; }
        
        public ICommand CheckNotificationPermissions { get; set; }
        
        public ICommand SkipStep { get; set; }
        
        private INavigation NavigationService { get; set; }

        public AllowPushNotificationViewModel(INavigation navigationService)
        {
            NavigationService = navigationService;
            
            GoBack = new Command(async () =>
            {
                NavigationService.PopAsync(true);
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
        
        public void AdvancePage()
        {
            NavigationService.PushAsync(new WalletCreationPage(), true);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}