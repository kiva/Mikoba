using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.UI.Pages;
using mikoba.UI.Pages.Onboarding;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public class WalletPinConfirmViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
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
        
        public string InstructionBlurb { get; set; }

        public ICommand GoBack { get; set; }
        
        public ICommand ConfirmPin { get; set; }
        
        private INavigation NavigationService { get; set; }
        
        public WalletPinConfirmViewModel(INavigation navigationService)
        {
            NavigationService = navigationService;
            NoError = true;
            
            GoBack = new Command(async () =>
            {
                await NavigationService.PopAsync(true);
            });
            
            ConfirmPin = new Command(async () =>
            {
                if (!string.IsNullOrEmpty(First) && !string.IsNullOrEmpty(Second) && !string.IsNullOrEmpty(Third) &&
                    !string.IsNullOrEmpty(Fourth))
                {
                    string toConfirm = $"{First}{Second}{Third}{Fourth}";
                    if (PIN.Equals(toConfirm))
                    {
                        NoMatch = false;
                        Application.Current.Properties["WalletPIN"] = toConfirm;
                        await NavigationService.PushAsync(new AllowCameraConfirmationPage(), true);
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

        public void SetPIN(string pin)
        {
            PIN = pin;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}