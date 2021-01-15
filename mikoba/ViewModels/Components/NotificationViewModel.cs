using System.Windows.Input;
using Hyperledger.Aries.Contracts;
using mikoba.Extensions;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using mikoba.Annotations;

namespace mikoba.ViewModels.Components
{
    public class NotificationViewModel: KivaBaseViewModel
    {
        public NotificationViewModel(
            IEventAggregator eventAggregator
        )
        {
            _eventAggregator = eventAggregator;
        }
        
        #region Services
        private readonly IEventAggregator _eventAggregator;
        #endregion
        
        public ICommand DismissNotification  => new Command(() =>
        {
            _eventAggregator.Publish(new CoreDispatchedEvent() {Type = DispatchType.NotificationDismissed});
        });

        public new event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}