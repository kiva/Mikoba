using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages.Onboarding
{
    public class WalletOwnerInputViewModel : MikobaBaseViewModel, INotifyPropertyChanged
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
                    SetOwnerError("");
                    await NavigationService.NavigateToAsync<WalletPinSetViewModel>();
                }
                else
                {
                    SetOwnerError("Wallet owner field cannot be empty.");
                    Console.WriteLine("Unable to set the wallet owner");
                }
            });
            
            
        }
        
        private string _owner = string.Empty;
        private string _ownerError = string.Empty;
        
        void SetOwnerError(string error)
        {
            OwnerError = error;
        }
        public string OwnerError
        {
            get
            {
                return _ownerError;
            }
            set
            {
                if (_ownerError != value)
                {
                    _ownerError = value;
                    OnPropertyChanged(nameof(OwnerError));
                }
            }
        }
        public string Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                if (_owner != value)
                {
                    _owner = value;
                    OnPropertyChanged(nameof(Owner));
                }
            }
        }

        #region Commands
        public ICommand GoBack { get; set; }
        
        public ICommand SetOwner { get; set; }
        
        #endregion

        public new event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
