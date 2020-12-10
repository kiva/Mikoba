using System.ComponentModel;

namespace mikoba.ViewModels
{
    public class CredentialRequestViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        private static CredentialRequestViewModel m_instance;


        public static CredentialRequestViewModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new CredentialRequestViewModel();
                }

                return m_instance;
            }
        }
    }
}
