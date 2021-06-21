using System.Collections.Generic;

namespace mikoba.ViewModels.SSI
{
    public class CredentialOverlayField
    {
        public string Name { get; set; }
        public Dictionary<string,string> Properties { get; set; }

        public string DisplayName
        {
            get
            {
                return Properties["name"];
            }
        }
            
        public string DataType
        {
            get
            {
                return Properties["dataType"];
            }
        }
    }
}