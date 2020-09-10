using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.UI.Pages;
using Sentry;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public class WalletOwnerInputViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        private string owner = string.Empty;

        public string Owner
        {
            get
            {
                return owner;
            }
            set
            {
                if (owner != value)
                {
                    owner = value;
                    OnPropertyChanged(nameof(Owner));
                }
            }
        }

        public ICommand GoBack { get; set; }
        
        public ICommand SetOwner { get; set; }
        
        private INavigation NavigationService { get; set; }

        public WalletOwnerInputViewModel(INavigation navigationService)
        {
            NavigationService = navigationService;
            GoBack = new Command(async () =>
            {
                await NavigationService.PopAsync(true);
            });
            
            SetOwner = new Command(async () =>
            {
                if (!Owner.Equals(string.Empty))
                {
                    Application.Current.Properties["WalletOwner"] = Owner;
                    await NavigationService.PushAsync(new WalletPinSetPage(), true);
                }
                else
                {
                    Console.WriteLine("Unable to set the wallet owner");
                }
            });
        }

        private string GetFirstName()
        {
            string firstName = Owner.Split(' ')[0];
            
            if (!string.IsNullOrEmpty(firstName))
            {
                return firstName;
            }
            
            return "Outis";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}