using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

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
