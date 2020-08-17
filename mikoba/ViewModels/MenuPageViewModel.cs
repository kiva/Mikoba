using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using mikoba.Annotations;
using Sentry;
using Xamarin.Forms;

namespace mikoba.UI
{
    public sealed class MenuPageViewModel : INotifyPropertyChanged
    {
        public INavigation NavigationService { get; private set; }
        public ICommand OpenConnectionCommand { get; set; }
        public ICommand ShowCredentialsListViewCommand { get; set; }
        public ICommand ShowScanQrCodePageCommand { get; set; }
        
        public MenuPageViewModel(INavigation service)
        {
            this.NavigationService = service;
            this.OpenConnectionCommand = new Command(async () =>
            {
                SentrySdk.CaptureEvent(new SentryEvent(){Message = "Open Connection"});
                await this.NavigationService.PushAsync(new ScanQrCodePage());
            });
            this.ShowCredentialsListViewCommand = new Command(async () =>
            {
                SentrySdk.CaptureEvent(new SentryEvent(){Message = "Show Credentials"});
                await this.NavigationService.PushAsync(new CredentialsListPage());
            });
            this.ShowScanQrCodePageCommand = new Command(async () =>
            {
                SentrySdk.CaptureEvent(new SentryEvent(){Message = "Scan QR Code"});
                await this.NavigationService.PushAsync(new ScanQrCodePage());
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
