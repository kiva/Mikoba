using System.ComponentModel;

namespace mikoba.ViewModels
{
    public class CredentialOfferReviewViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        private static CredentialOfferReviewViewModel m_instance;


        public static CredentialOfferReviewViewModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new CredentialOfferReviewViewModel();
                }

                return m_instance;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
