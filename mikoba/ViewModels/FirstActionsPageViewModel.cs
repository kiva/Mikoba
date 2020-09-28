using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.UI;
using mikoba.UI.Pages;
using mikoba.UI.Pages.Connections;
using mikoba.UI.Pages.Wallet;
using Sentry;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public sealed class FirstActionsPageViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        private static FirstActionsPageViewModel m_instance;
        public INavigation NavigationService { get; set; }
        public ICommand OpenConnectionCommand { get; set; }
        public ICommand ShowWalletHomePageCommand { get; set; }
        public ICommand ShowCredentialsCommand { get; set; }
        
        public string WelcomeText { get; set; }

        public static FirstActionsPageViewModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new FirstActionsPageViewModel();
                }

                return m_instance;
            }
        }

        public FirstActionsPageViewModel()
        {
            this.OpenConnectionCommand = new Command(async () =>
            {
                await this.NavigationService.PushAsync(new QrScanPage());
            });

            this.ShowWalletHomePageCommand = new Command(async () =>
            {
                await this.NavigationService.PushAsync(new FirstActionsPage());
            });
            
            this.ShowCredentialsCommand = new Command(async () =>
            {
                await NavigationService.PushAsync(new WalletPage());
            });

            var data = Application.Current.Properties["WalletCreationDate"];
            WelcomeText = String.Format("Hello {0}, welcome to your new Wallet.  Get started by receiving your first ID.", Application.Current.Properties[AppConstant.FullName]);
            OnPropertyChanged(nameof(WelcomeText));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
