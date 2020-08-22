using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using mikoba.Annotations;
using mikoba.Models;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public class CredentialsListViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public Command AddCredentialCommand { get; set; }
        public ObservableCollection<Credential> Credentials { get; set; }

        public static CredentialsListViewModel Instance { get; }

        static CredentialsListViewModel()
        {
            Instance = new CredentialsListViewModel();
            Console.WriteLine(Instance.Credentials);
        }
        public CredentialsListViewModel()
        {
            Credentials = new ObservableCollection<Credential>();
        }

        public void AddCredential(Credential info)
        {
            Credentials.Add(info);
            OnPropertyChanged("Credentials");
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
