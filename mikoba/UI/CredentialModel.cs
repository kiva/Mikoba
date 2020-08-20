namespace mikoba.UI
{
    public class CredentialModel
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
}
