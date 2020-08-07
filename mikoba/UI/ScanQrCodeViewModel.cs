using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace mikoba.UI
{
    public class ScanQrCodeViewModel : INotifyPropertyChanged
    {
        public ScanQrCodeViewModel()
        {
            UpdateToDoListCommand = new Command(UpdateToDoList);
        }
        public ICommand UpdateToDoListCommand { get; }

        string name = string.Empty;
        public string Name
        {
            get => name;
            set
            {
                if (name == value)
                    return;
                name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(DisplayName));
            }
        }
        public string DisplayName => $"Name Entered: {Name}";

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        List<string> ToDoList = new System.Collections.Generic.List<string>();
        public String ToDoListArr { get; set; }

        void UpdateToDoList()
        {
            ToDoList.Add(Name);
            ToDoListArr = string.Join(",", ToDoList.ToArray());
            OnPropertyChanged(nameof(DisplayToDoList));
        }
        public string DisplayToDoList => $"The to do list is: {ToDoListArr}";

    }
}
