using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using mikoba.ViewModels;
using Xamarin.Forms;
using System.Windows.Input;
using Hyperledger.Aries.Features.DidExchange;
using mikoba.UI.Pages;
using mikoba.UI.Pages.Connections;
using mikoba.UI.Pages.Settings;

namespace mikoba.UI.ViewModels
{
    public class WalletPageViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        private readonly IConnectionService _connectionService;
        
        public ObservableCollection<CredentialModel> Credentials { get; set; }
        public ObservableCollection<WalletActionModel> WalletActions { get; set; }

        public int MenuOptionsHeight { get; set; }
        public ICommand ScanCodeCommand { get; set; }

        public ICommand ShowCredentialOfferReviewPageCommand { get; set; }

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
            this.ScanCodeCommand = new Command(async () => { await NavigationService.PushAsync(new QrScanPage()); });
            this.ShowCredentialOfferReviewPageCommand = new Command(async () =>
            {
                await NavigationService.PushAsync(new CredentialOfferReviewPage());
            });
            Credentials = new ObservableCollection<CredentialModel>();
            WalletActions = new ObservableCollection<WalletActionModel>();
            LoadDefaultCredentials();
            LoadDefaultActions();
            this.MenuOptionsHeight = 90 * WalletActions.Count;
        }

        private void LoadDefaultActions()
        {
            WalletActions.Add(new WalletActionModel
            {
                ActionLabel = "Save and secure your wallet",
                RightIcon = this.RightCaret,
                LeftIcon = this.Secure,
            });
            // WalletActions.Add(new WalletActionModel
            // {
            //     ActionLabel = "1 offer",
            //     RightIcon = this.RightCaretYellow,
            //     LeftIcon = this.Clock,
            //     ActionCommand = new Command(async () =>
            //     {
            //         await NavigationService.PushAsync(new CredentialOfferReviewPage());
            //     })
            // });
            // WalletActions.Add(new WalletActionModel
            // {
            //     ActionLabel = "1 request",
            //     RightIcon = this.RightCaretYellow,
            //     LeftIcon = this.Clock,
            //     ActionCommand = new Command(async () =>
            //     {
            //         await NavigationService.PushAsync(new CredentialRequestPage());
            //     })
            // });
        }
        
        private void LoadDefaultCredentials()
        {
            // Credentials.Add(new CredentialModel
            // {
            //     Organization = "GOVERNMENT",
            //     MemberId = "Sixth Bank",
            //     LogoName = "mikoba.Images.kiva.svg"
            // });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override Task InitializeAsync(object navigationDat)
        {
            Console.WriteLine("Custom Code!");
            return Task.FromResult(0);
        }
    }
}
