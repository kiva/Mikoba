using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using mikoba.Annotations;
using mikoba.UI.Pages;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public class FingerprintLoginViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public FingerprintLoginViewModel(INavigation navigationService)
        {
            NavigationService = navigationService;
        }
        
        
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}