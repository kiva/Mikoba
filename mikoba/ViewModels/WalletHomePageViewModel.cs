using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.UI;
using mikoba.UI.Pages;
using Sentry;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public sealed class WalletHomePageViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        private static WalletHomePageViewModel m_instance;
        public INavigation NavigationService { get; set; }
        public ICommand OpenConnectionCommand { get; set; }
        public ICommand ShowWalletHomePageCommand { get; set; }
        public ICommand ShowCredentialsCommand { get; set; }

        public string WalletCreationText { get; set; }

        public static WalletHomePageViewModel Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new WalletHomePageViewModel();
                }

                return m_instance;
            }
        }

        public WalletHomePageViewModel()
        {
            this.OpenConnectionCommand = new Command(async () =>
            {
                SentrySdk.CaptureEvent(new SentryEvent() {Message = "Open Connection"});
                await this.NavigationService.PushAsync(new QrScanPage());
            });

            this.ShowWalletHomePageCommand = new Command(async () =>
            {
                SentrySdk.CaptureEvent(new SentryEvent() {Message = "Scan QR Code"});
                await this.NavigationService.PushAsync(new WalletHomePage());
            });
            
            this.ShowCredentialsCommand = new Command(async () =>
            {
                await NavigationService.PushAsync(new WalletPage());
            });
            
            this.DestroyWalletCommand = new Command(async () =>
            {
                Application.Current.Properties.Clear();
                await Application.Current.SavePropertiesAsync();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            });
            
            var data = Application.Current.Properties["WalletCreationDate"];
            var strDate = ((DateTime)data).ToString("D", CultureInfo.CurrentCulture);
            WalletCreationText = String.Format("Wallet Created on {0}", strDate);
            OnPropertyChanged(nameof(WalletCreationText));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
