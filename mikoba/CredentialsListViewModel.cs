using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace mikoba
{
    public class CredentialsListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Credential> Credentials { get; set; }
        public class Credential
        {
            private string _issued;
            private string _organization;
            private string _memberId;
            private string _imageUrl;

            public string Issued
            {
                get { return _issued; }
                set { _issued = value; }
            }
            public string Organization
            {
                get { return _organization; }
                set { _organization = value; }
            }
                
            public string MemberId
            {
                get { return _memberId; }
                set { _memberId = value; }
            }
                
            public string ImageUrl
            {
                get { return _imageUrl; }
                set { _imageUrl = value; }
            }
        }

        public CredentialsListViewModel()
        {
            Credentials = new ObservableCollection<Credential>();
            Credentials.Add(new Credential
            {
              Issued = "Date Here",
              Organization = "Kiva",
              MemberId = "3874829",
              ImageUrl = "image.png"
            });
            Credentials.Add(new Credential
            {
                Issued = "Hello World",
                Organization = "Org",
                MemberId = "543245",
                ImageUrl = "ncra.png"
            });
        }

        public Command AddCredential()
        {
            Credentials.Add(new Credential
            {
                Issued = "Hello World",
                Organization = "Org",
                MemberId = "543245",
                ImageUrl = "ncra.png"
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}