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
    public class WalletPinSetViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public WalletPinSetViewModel(INavigationService navigationService)
         : base("Pin Set", navigationService)
        {
            InstructionBlurb = $"{GetFirstName()}, create a passcode to keep your Wallet secure.";
            
            GoBack = new Command(async () =>
            {
                await NavigationService.NavigateBackAsync();
            });
            
            GoToPINConfirmation = new Command(async () =>
            {
                if (!string.IsNullOrEmpty(First) && !string.IsNullOrEmpty(Second) && !string.IsNullOrEmpty(Third) &&
                    !string.IsNullOrEmpty(Fourth))
                {
                    await NavigationService.NavigateToAsync<WalletPinConfirmationViewModel>($"{First}{Second}{Third}{Fourth}");
                }
            });
        }
        
        private string GetFirstName()
        {
            string name = Preferences.Get(AppConstant.FullName, "Outis");
            string firstName = name.Split(' ')[0];
            
            if (!string.IsNullOrEmpty(firstName))
            {
                name = firstName;
            }

            return name;
        }

        #region UI Properties
        private string first = string.Empty;
        private string second = string.Empty;
        private string third = string.Empty;
        private string fourth = string.Empty;

        public string InstructionBlurb { get; set; }

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
                    OnPropertyChanged(nameof(first));
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
                    OnPropertyChanged(nameof(second));
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
                    OnPropertyChanged(nameof(third));
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
                    OnPropertyChanged(nameof(fourth));
                }
            }
        }
        
        #endregion
        
        #region Commands
        public ICommand GoBack { get; set; }
        
        public ICommand GoToPINConfirmation { get; set; }
        
        #endregion

        public new event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
