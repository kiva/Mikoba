using System.Collections.ObjectModel;
using System.ComponentModel;
using mikoba.ViewModels;
using Xamarin.Forms;

namespace mikoba.UI.ViewModels
{
    public class WalletPageViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<CredentialModel> Credentials { get; set; }
        
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
                Organization = "First Bank of Sierra Leone",
                MemberId = "GOVERNMENT",
                LogoName = "mikoba.Images.government.svg"
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
