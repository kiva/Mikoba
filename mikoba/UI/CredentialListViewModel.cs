using System.Collections.ObjectModel;
using System.ComponentModel;
using mikoba.ViewModels;
using Xamarin.Forms;

namespace mikoba.UI
{
    public class CredentialListViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<CredentialModel> Credentials { get; set; }
        
        public Command AddCredential { get; private set; }

        private static CredentialListViewModel m_instance;
        public static CredentialListViewModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new CredentialListViewModel();
                }
                return m_instance;
            }
        }
        
        public CredentialListViewModel()
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
