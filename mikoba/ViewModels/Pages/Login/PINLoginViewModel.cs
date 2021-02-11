using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using Hyperledger.Aries.Contracts;
using mikoba.Annotations;
using mikoba.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages.Login
{
    public class PINLoginViewModel : MikobaBaseViewModel
    {
        public PINLoginViewModel(
            INavigationService navigationService) : base("PIN Login", navigationService)
        {
            NoError = true;
            
            Login = new Command(async () =>
            {
                string entered = $"{First}{Second}{Third}{Fourth}";
                if (Preferences.Get(AppConstant.PIN, "xxxxx") == entered)
                {
                    NoMatch = false;
                    await NavigationService.NavigateToAsync<WalletPageViewModel>();
                }
                else
                {
                    NoMatch = true;
                    await Task.Delay(3000);
                    NoMatch = false;
                }
            });
        }
        
        #region UI Properties
        private string first = string.Empty;
        private string second = string.Empty;
        private string third = string.Empty;
        private string fourth = string.Empty;
        private bool nomatch;
        
        public string PIN { get; set; }

        public string First
        {
            get
            {
                return first;
            }
            set
            {
                if (first != value)
                {
                    first = value;
                    OnPropertyChanged(nameof(First));
                }
            }
        }
        
        public string Second
        {
            get
            {
                return second;
            }
            set
            {
                if (second != value)
                {
                    second = value;
                    OnPropertyChanged(nameof(Second));
                }
            }
        }
        
        public string Third
        {
            get
            {
                return third;
            }
            set
            {
                if (third != value)
                {
                    third = value;
                    OnPropertyChanged(nameof(Third));
                }
            }
        }
        
        public string Fourth
        {
            get
            {
                return fourth;
            }
            set
            {
                if (fourth != value)
                {
                    fourth = value;
                    OnPropertyChanged(nameof(Fourth));
                }
            }
        }

        public bool NoMatch
        {
            get
            {
                return nomatch;
            }
            set
            {
                if (nomatch != value)
                {
                    nomatch = value;
                    NoError = !value;
                    OnPropertyChanged(nameof(NoMatch));
                    OnPropertyChanged(nameof(NoError));
                }
            }
        }
        
        public bool NoError { get; set; }

        #endregion

        #region Commands
        
        public ICommand Login { get; set; }

        #endregion

        #region Lifecyle
        
        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is string pin)
            {
                PIN = pin;
            }
            return base.InitializeAsync(navigationData);
        }
        
        #endregion
        
        public new event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
