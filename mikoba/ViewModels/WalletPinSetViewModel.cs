using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using mikoba.Annotations;
using mikoba.UI.Pages;
using mikoba.UI.Pages.Onboarding;
using Xamarin.Forms;

namespace mikoba.ViewModels
{
    public class WalletPinSetViewModel : KivaBaseViewModel, INotifyPropertyChanged
    {
        private string first = string.Empty;
        private string second = string.Empty;
        private string third = string.Empty;
        private string fourth = string.Empty;

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
        
        public string InstructionBlurb { get; set; }

        public ICommand GoBack { get; set; }
        
        public ICommand GoToPINConfirmation { get; set; }
        
        private INavigation NavigationService { get; set; }

        public WalletPinSetViewModel(INavigation navigationService)
        {
            NavigationService = navigationService;
            GoBack = new Command(async () =>
            {
                await NavigationService.PopAsync(true);
            });
            
            GoToPINConfirmation = new Command(async () =>
            {
                if (!string.IsNullOrEmpty(First) && !string.IsNullOrEmpty(Second) && !string.IsNullOrEmpty(Third) &&
                    !string.IsNullOrEmpty(Fourth))
                {
                    await NavigationService.PushAsync(new WalletPinConfirmationPage($"{First}{Second}{Third}{Fourth}"), true);
                }
            });
        }

        public void SetFirstName(string name)
        {
            InstructionBlurb = $"{name}, create a PIN to keep your Wallet secure";
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
