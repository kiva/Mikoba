using mikoba.ViewModels;

namespace mikoba.UI
{
    public class CredentialModel : MikobaBaseViewModel
    {
        private string _organization;
        private string _memberId;
        private string _logo;
        
        
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

        public string LogoName
        {
            get { return _logo; }
            set { _logo = value; }
        }
    }
}
