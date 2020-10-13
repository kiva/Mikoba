using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using mikoba.Annotations;

namespace mikoba.ViewModels
{
    public class CredentialRequestCardViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public ICommand OnClickShareCommand  { get; set; }
        public ICommand OnClickDeclineCommand { get; set; }

        private bool _isShared;
        public bool IsShared
        {
            get { return _isShared; }
            set
            {
                _isShared = value;
                OnPropertyChanged(nameof(IsShared));
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
        
        private bool _isRequested;
        public bool IsRequested
        {
            get { return _isRequested; }
            set
            {
                _isRequested = value;
                _isReceived = !value;
                OnPropertyChanged(nameof(IsRequested));
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
                _isRequested = !value;
                OnPropertyChanged(nameof(IsRequested));
                OnPropertyChanged(nameof(IsReceived));
            }
        }
        
        private static CredentialRequestCardViewModel m_instance;
        public static CredentialRequestCardViewModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new CredentialRequestCardViewModel();
                }
                return m_instance;
            }
        }
        
        public CredentialRequestCardViewModel()
        {
            this.OnClickShareCommand = new Command(async () =>
            {
                IsShared = true;
                IsRequested = false;
            });
            this.OnClickDeclineCommand = new Command(() =>
            {
                IsDeclined = true;
                IsRequested = false;
            });
            
            IsRequested = true;
            IsShared = false;
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