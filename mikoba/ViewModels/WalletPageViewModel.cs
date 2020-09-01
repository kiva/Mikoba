using System.Collections.ObjectModel;
using System.ComponentModel;
using mikoba.ViewModels;
using Xamarin.Forms;
using System.Windows.Input;

namespace mikoba.UI.ViewModels
{
    public class WalletPageViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<CredentialModel> Credentials { get; set; }
        
        public ICommand ShowCredentialsCommand { get; set; }

        public Command AddCredential { get; private set; }

        private static WalletPageViewModel m_instance;
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
