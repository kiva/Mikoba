using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using Hyperledger.Indy.AnonCredsApi;
using mikoba.Annotations;
using mikoba.UI.Pages;
using mikoba.UI.ViewModels;

namespace mikoba.ViewModels
{
    public class CredentialOfferCardViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public ICommand OnClickAcceptCommand  { get; set; }
        public ICommand OnClickDeclineCommand { get; set; }

        private bool _isAccepted;
        public bool IsAccepted
        {
            get { return _isAccepted; }
            set
            {
                _isAccepted = value;
                OnPropertyChanged(nameof(IsAccepted));
            }
        }
        
        private bool _isDeclined;
        public bool IsDeclined
        {
            get { return _isDeclined; }
            set
            {
                _isDeclined = value;
                OnPropertyChanged(nameof(IsDeclined));
            }
        }
        
        private bool _isOffered;
        public bool IsOffered
        {
            get { return _isOffered; }
            set
            {
                _isOffered = value;
                _isReceived = !value;
                OnPropertyChanged(nameof(IsOffered));
                OnPropertyChanged(nameof(IsReceived));
            }
        }

        private bool _isReceived;
        public bool IsReceived
        {
            get { return _isReceived; }
            set
            {
                _isReceived = value;
                _isOffered = !value;
                OnPropertyChanged(nameof(IsOffered));
                OnPropertyChanged(nameof(IsReceived));
            }
        }
        
        private static CredentialOfferCardViewModel m_instance;
        public static CredentialOfferCardViewModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new CredentialOfferCardViewModel();
                }
                return m_instance;
            }
        }
        
        public CredentialOfferCardViewModel()
        {
            this.OnClickAcceptCommand = new Command(async () =>
            {
                IsAccepted = true;
                IsOffered = false;
                await WalletPageViewModel.Instance.NavigationService.PopAsync();
            });
            this.OnClickDeclineCommand = new Command(async () =>
            {
                //TODO: Create Message Service
                //await DisplayAlert ("Alert", "You have been alerted", "OK");
            });
            
            IsOffered = true;
            IsAccepted = false;
            IsDeclined = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
