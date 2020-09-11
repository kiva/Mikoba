using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.UI.Pages;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public class WalletPinSetViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        private string pin = string.Empty;

        public string PIN
        {
            get
            {
                return pin;
            }
            set
            {
                if (pin != value)
                {
                    pin = value;
                    OnPropertyChanged(nameof(pin));
                }
            }
        }
        
        public string InstructionBlurb { get; set; }

        public ICommand GoBack { get; set; }
        
        public ICommand SetOwner { get; set; }
        
        private INavigation NavigationService { get; set; }

        public WalletPinSetViewModel(INavigation navigationService)
        {
            NavigationService = navigationService;
            GoBack = new Command(async () =>
            {
                await NavigationService.PopAsync(true);
            });
            
            // SetOwner = new Command(async () =>
            // {
            //     if (!Owner.Equals(string.Empty))
            //     {
            //         Application.Current.Properties["WalletOwner"] = Owner;
            //         await NavigationService.PushAsync(new WalletPinSetPage(GetFirstName()), true);
            //     }
            //     else
            //     {
            //         Console.WriteLine("Unable to set the wallet owner");
            //     }
            // });
        }

        public void SetFirstName(string name)
        {
            InstructionBlurb = $"{name}, create a pin to keep your wallet secure";
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}