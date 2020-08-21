using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace mikoba.UI
{
    public class CredentialsListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CredentialModel> Credentials { get; set; }
        
        public Command AddCredential { get; private set; }

        private static CredentialsListViewModel m_instance;
        public static CredentialsListViewModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new CredentialsListViewModel();
                }
                return m_instance;
            }
        }
        
        public CredentialsListViewModel()
        {
            Credentials = new ObservableCollection<CredentialModel>();
            AddCredential = new Command(() =>
            {
                Credentials.Add(new CredentialModel
                {
                    Issued = "Hello World",
                    Organization = "Org",
                    MemberId = "543245",
                    ImageUrl = "ncra.png"
                });
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
