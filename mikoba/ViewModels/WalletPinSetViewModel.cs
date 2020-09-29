using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.Services;
using mikoba.UI.Pages;
using mikoba.UI.Pages.Onboarding;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public class WalletPinSetViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        public WalletPinSetViewModel(INavigationService navigationService)
         : base("Pin Set", navigationService)
        {
            GoBack = new Command(async () =>
            {
                await NavigationService.NavigateBackAsync();
            });
            
            GoToPINConfirmation = new Command(async () =>
            {
                if (!string.IsNullOrEmpty(First) && !string.IsNullOrEmpty(Second) && !string.IsNullOrEmpty(Third) &&
                    !string.IsNullOrEmpty(Fourth))
                {
                    await NavigationService.NavigateToAsync<WalletPinConfirmViewModel>($"{First}{Second}{Third}{Fourth}");
                }
            });
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

        #region Lifecyle
        
        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is string name)
            {
                InstructionBlurb = $"{name}, create a PIN to keep your Wallet secure";    
            }
            return base.InitializeAsync(navigationData);
        }
        
        #endregion
        
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
