using System.Collections.ObjectModel;
using System.ComponentModel;
using mikoba.Services;
using mikoba.ViewModels;

namespace mikoba.UI.ViewModels
{
    public class SettingsPageViewModel : MikobaBaseViewModel
    {
        public ObservableCollection<WalletActionModel> SettingsActions { get; set; }

        public SettingsPageViewModel(INavigationService navigationService)
             : base("Settings", navigationService)
        {
            SettingsActions = new ObservableCollection<WalletActionModel>();
            SettingsActions.Add(new WalletActionModel
            {
                ActionLabel = "General Settings",
                RightIcon = this.RightCaret,
                LeftIcon = this.Secure,
            });
            SettingsActions.Add(new WalletActionModel
            {
                ActionLabel = "Profile",
                RightIcon = this.RightCaret,
                LeftIcon = this.Secure,
            });            
            SettingsActions.Add(new WalletActionModel
            {
                ActionLabel = "Network",
                RightIcon = this.RightCaret,
                LeftIcon = this.Secure,
            });            
            SettingsActions.Add(new WalletActionModel
            {
                ActionLabel = "Privacy Policy",
                RightIcon = this.RightCaret,
                LeftIcon = this.Secure,
            });            
            SettingsActions.Add(new WalletActionModel
            {
                ActionLabel = "Terms of Service",
                RightIcon = this.RightCaret,
                LeftIcon = this.Secure,
            });
        }
    }
}
