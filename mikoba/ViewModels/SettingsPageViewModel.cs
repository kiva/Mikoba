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
        public INavigation NavigationService { get; private set; }
        public ObservableCollection<CredentialModel> Credentials { get; set; }
        
        public ICommand ShowCredentialsCommand { get; set; }
        public ICommand ShowCredentialOfferCommand { get; set; }

        public Command AddCredential { get; private set; }

        private static SettingsPageViewModel m_instance;
        public SettingsPageViewModel(INavigation navigationService)
        {
            this.NavigationService = navigationService;
        }
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
            this.ShowCredentialsCommand = new Command(async () =>
            {
                await NavigationService.PushAsync(new WalletPage());
            });
           
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
