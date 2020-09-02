using System.Collections.ObjectModel;
using System.ComponentModel;
using mikoba.ViewModels;
using Xamarin.Forms;
using System.Windows.Input;
using mikoba.UI.Pages;

namespace mikoba.UI.ViewModels
{
    public class WalletPageViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public INavigation NavigationService { get; private set; }
        public ObservableCollection<CredentialModel> Credentials { get; set; }
        
        public ICommand ShowCredentialsCommand { get; set; }
        public ICommand ShowCredentialOfferCommand { get; set; }

        public Command AddCredential { get; private set; }

        private static WalletPageViewModel m_instance;
        public WalletPageViewModel(INavigation navigationService)
        {
            this.NavigationService = navigationService;
        }
        public static WalletPageViewModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new WalletPageViewModel();
                }
                return m_instance;
            }
        }
        
        public WalletPageViewModel()
        {
            this.ShowCredentialsCommand = new Command(async () =>
            {
                await NavigationService.PushAsync(new WalletPage());
            });
            this.ShowCredentialOfferCommand = new Command(async () =>
            {
                await NavigationService.PushAsync(new CredentialOfferReviewPage());
            });
            Credentials = new ObservableCollection<CredentialModel>();
            LoadDefaultCredentials();
        }

        public void LoadDefaultCredentials()
        {
            Credentials.Add(new CredentialModel
            {
                Organization = "GOVERNMENT",
                MemberId = "First Bank",
                LogoName = "mikoba.Images.kiva.svg"
            });
            Credentials.Add(new CredentialModel
            {
                Organization = "GOVERNMENT",
                MemberId = "Second Bank",
                LogoName = "mikoba.Images.kiva.svg"
            });
            Credentials.Add(new CredentialModel
            {
                Organization = "GOVERNMENT",
                MemberId = "Third Bank",
                LogoName = "mikoba.Images.kiva.svg"
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
