using System.ComponentModel;

namespace mikoba.UI.ViewModels
{
    public class CreateConnectionViewModel : INotifyPropertyChanged
    {
        public CreateConnectionViewModel()
        {
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
