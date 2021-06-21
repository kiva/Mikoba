namespace mikoba.ViewModels.SSI
{
    public class SSICredentialAttribute
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string DisplayName
        {
            get
            {
                return SSICredentialViewModel.FormatCredentialName(this.Name);
            }
        }
        public string DisplayValue
        {
            get
            {
                return SSICredentialViewModel.FormatCredentialValue(this.Name, this.Value);
            }
        }

        public object Value { get; set; }

        public string FileExt { get; set; }

        public string Date { get; set; }
    }
}
