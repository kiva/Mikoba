using mikoba.Services;
using mikoba.ViewModels.Pages;
using mikoba.ViewModels.SSI;
using ReactiveUI;
using Xamarin.Forms;

namespace mikoba.ViewModels.Components
{
    public class EntryViewModel : KivaBaseViewModel
    {
        public EntryViewModel(INavigationService navigationService)
            : base("Action Hub", navigationService)
        {
            
        }

        public SSIConnectionViewModel Connection { get; set; }
        public SSICredentialViewModel Credential { get; set; }

        public void Setup()
        {
            if (this.Credential != null)
            {
                DisplayName = "Civil Registry Office";
                Tag = "Issue Date: " + this.Credential._credential.CreatedAtUtc?.ToString("MM/dd/yy");
                this.OrganizationType = "Government";
            }
            else if (Connection != null)
            {
                DisplayName = Connection.ConnectionName;
                ImageUrl = Connection.ConnectionImageUrl;
            }
        }

        #region Commands

        public Command OpenCommand
        {
            get => new Command(async () => { await NavigationService.NavigateToAsync<EntryHubPageViewModel>(this); });
        }

        #endregion

        #region UI Properties

        private string _displayName;

        public string DisplayName
        {
            get => _displayName;
            set => this.RaiseAndSetIfChanged(ref _displayName, value);
        }

        private string _organizationType;

        public string OrganizationType
        {
            get => _organizationType;
            set => this.RaiseAndSetIfChanged(ref _organizationType, value);
        }

        private string _imageUrl;

        public string ImageUrl
        {
            get => _imageUrl;
            set => this.RaiseAndSetIfChanged(ref _imageUrl, value);
        }
        
        private string _iconIdentifier;

        public string IconIdentifier
        {
            get => _iconIdentifier;
            set => this.RaiseAndSetIfChanged(ref _iconIdentifier, value);
        }
        
        private string _tag;

        public string Tag
        {
            get => _tag;
            set => this.RaiseAndSetIfChanged(ref _tag, value);
        }

        #endregion
    }
}
