using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.UI;
using Sentry;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public sealed class WalletCreationViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public INavigation NavigationService { get; private set; }

        public WalletCreationViewModel(INavigation navigationService)
        {
            this.NavigationService = navigationService;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
