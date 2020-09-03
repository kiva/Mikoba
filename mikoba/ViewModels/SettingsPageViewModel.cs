using System.Collections.ObjectModel;
using System.ComponentModel;
using mikoba.ViewModels;
using Xamarin.Forms;
using System.Windows.Input;
using mikoba.UI.Pages;

namespace mikoba.UI.ViewModels
{
    public class SettingsPageViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        private static SettingsPageViewModel m_instance;
        
        public ObservableCollection<WalletActionModel> SettingsActions { get; set; }
        
        public static SettingsPageViewModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new SettingsPageViewModel();
                }
                return m_instance;
            }
        }
        
        public SettingsPageViewModel()
        {
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}