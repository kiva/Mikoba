using System.Collections.ObjectModel;
using System.ComponentModel;
using mikoba.ViewModels;
using Xamarin.Forms;
using System.Windows.Input;
using mikoba.UI.Pages;

namespace mikoba.UI.ViewModels
{
    public class WalletPageViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public INavigation NavigationService { get;  set; }
        public ObservableCollection<CredentialModel> Credentials { get; set; }
        public ObservableCollection<WalletActionModel> WalletActions { get; set; }

        public ICommand ScanCodeCommand { get; set; }

        public Command SettingsCommand { get; private set; }

        private static WalletPageViewModel m_instance;

        public static WalletPageViewModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new WalletPageViewModel();
                }

                return m_instance;
            }
        }

        public WalletPageViewModel()
        {
            this.SettingsCommand = new Command(async () =>
            {
                await NavigationService.PushAsync(new SettingsPage());
            });
            this.ScanCodeCommand = new Command(async () =>
            {
                await NavigationService.PushAsync(new QrScanPage());
            });
            Credentials = new ObservableCollection<CredentialModel>();
            WalletActions = new ObservableCollection<WalletActionModel>();
            LoadDefaultCredentials();
            LoadDefaultActions();
        }

        private void LoadDefaultActions()
        {
            WalletActions.Add(new WalletActionModel
            {
                ActionLabel = "Save and secure your wallet",
                RightIcon = this.RightCaret,
                LeftIcon = this.Secure,
            });
            WalletActions.Add(new WalletActionModel
            {
                ActionLabel = "1 offer",
                RightIcon = this.RightCaretYellow,
                LeftIcon = this.Clock,
                ActionCommand = new Command(async () =>
                {
                    await NavigationService.PushAsync(new QrScanPage());
                })
            });
        }

        private void LoadDefaultCredentials()
        {
            Credentials.Add(new CredentialModel
            {
                Organization = "GOVERNMENT",
                MemberId = "First Bank",
                LogoName = "mikoba.Images.kiva.svg"
            });
            Credentials.Add(new CredentialModel
            {
                Organization = "GOVERNMENT",
                MemberId = "Second Bank",
                LogoName = "mikoba.Images.kiva.svg"
            });
            Credentials.Add(new CredentialModel
            {
                Organization = "GOVERNMENT",
                MemberId = "Third Bank",
                LogoName = "mikoba.Images.kiva.svg"
            });
            Credentials.Add(new CredentialModel
            {
                Organization = "GOVERNMENT",
                MemberId = "Third Bank",
                LogoName = "mikoba.Images.kiva.svg"
            });
            Credentials.Add(new CredentialModel
            {
                Organization = "GOVERNMENT",
                MemberId = "Third Bank",
                LogoName = "mikoba.Images.kiva.svg"
            });
            Credentials.Add(new CredentialModel
            {
                Organization = "GOVERNMENT",
                MemberId = "Third Bank",
                LogoName = "mikoba.Images.kiva.svg"
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
