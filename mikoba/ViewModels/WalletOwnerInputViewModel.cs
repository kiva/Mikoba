using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.Services;
using mikoba.UI.Pages;
using mikoba.UI.Pages.Onboarding;
using Sentry;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public class WalletOwnerInputViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public WalletOwnerInputViewModel(INavigationService navigationService)
            : base("Owner Input", navigationService)
        {
            GoBack = new Command(async () =>
            {
                await NavigationService.NavigateBackAsync();
            });
            
            SetOwner = new Command(async () =>
            {
                if (!Owner.Equals(string.Empty))
                {
                    Preferences.Set(AppConstant.FullName, Owner);
                    
                    await NavigationService.NavigateToAsync<WalletPinSetViewModel>(GetFirstName());
                }
                else
                {
                    Console.WriteLine("Unable to set the wallet owner");
                }
            });
        }
        
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

        #region Commands
        public ICommand GoBack { get; set; }
        
        public ICommand SetOwner { get; set; }
        
        #endregion

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
