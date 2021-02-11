using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.Services;
using mikoba.UI.Pages;
using mikoba.UI.Pages.Onboarding;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mikoba.ViewModels.Pages.Onboarding
{
    public class WalletPinConfirmationViewModel : MikobaBaseViewModel, INotifyPropertyChanged
    {
        public WalletPinConfirmationViewModel(INavigationService navigationService)
            : base ("Confirm Pin", navigationService)
        {
            NoError = true;
            ConfirmPin = new Command(async () =>
            {
                if (!string.IsNullOrEmpty(First) && !string.IsNullOrEmpty(Second) && !string.IsNullOrEmpty(Third) &&
                    !string.IsNullOrEmpty(Fourth))
                {
                    string toConfirm = $"{First}{Second}{Third}{Fourth}";
                    if (PIN.Equals(toConfirm))
                    {
                        NoMatch = false;
                        Preferences.Set(AppConstant.PIN, toConfirm);
                        await NavigationService.NavigateToAsync<AllowFingerprintViewModel>();
                    }
                    else
                    {
                        NoMatch = true;
                        await Task.Delay(3000);
                        NoMatch = false;
                    }
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

        public int FocusDelay => 300;

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
        public ICommand GoBack { get; set; }
        
        public ICommand ConfirmPin { get; set; }
        
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
