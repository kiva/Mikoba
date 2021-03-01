using System.ComponentModel;

namespace mikoba.ViewModels
{
    public class CredentialRequestViewModel : MikobaBaseViewModel, INotifyPropertyChanged
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
