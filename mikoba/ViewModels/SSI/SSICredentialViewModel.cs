using System;
using Hyperledger.Aries.Features.IssueCredential;
using mikoba.Extensions;
using mikoba.Services;
using ReactiveUI;
using Xamarin.Essentials;

namespace mikoba.ViewModels.SSI
{
    public class SSICredentialViewModel : KivaBaseViewModel
    {
        public SSICredentialViewModel()
        {
            Attributes = new RangeEnabledObservableCollection<SSICredentialAttribute>();
        }
        public SSICredentialViewModel(CredentialRecord credential)
        {
            _credential = credential;
            Attributes = new RangeEnabledObservableCollection<SSICredentialAttribute>();
            foreach (var cred in _credential.CredentialAttributesValues)
            {
                Console.WriteLine("credential-name: " + cred.Name);
                Attributes.Add(new SSICredentialAttribute()
                {
                    Name = cred.Name,
                    Value = cred.Value,
                });
            }
        }

        public static string FormatCredentialName(string source)
        {
            switch (source)
            {
                case "nationalId": 
                    return "National Id";
                case "firstName": 
                    return "First Name";
                case "lastName": 
                    return "Last Name";
                case "dateOfBirth": 
                    return "Birth Date";
                case "birthDate": 
                    return "BirthDate";
                default: 
                    return source;
            }
        }
        
        public SSICredentialViewModel(
            INavigationService navigationService,
            CredentialRecord credential
        ) : base(
            nameof(SSICredentialViewModel),
            navigationService
        ) {
            _credential = credential;
            Console.WriteLine("credential-id:"  + credential.Id);
            Preferences.Set("credential-id", credential.Id);
            Attributes = new RangeEnabledObservableCollection<SSICredentialAttribute>();
            foreach (var cred in _credential.CredentialAttributesValues)
            {
                Attributes.Add(new SSICredentialAttribute()
                {
                    Name = cred.Name,
                    Value = cred.Value,
                });
            }
        }
        
        public bool IsAccepted
        {
            get
            {
                return Preferences.Get(_credential.Id + "-Accepted", false);
            }
            set
            {
                Preferences.Set(_credential.Id + "-Accepted", value);
            }
        }
        
        public readonly CredentialRecord _credential;
        
        #region UI Properties
        private string _credentialName;
        public string CredentialName
        {
            get => _credentialName;
            set => this.RaiseAndSetIfChanged(ref _credentialName, value);
        }

        private string _credentialType;
        public string CredentialType
        {
            get => _credentialType;
            set => this.RaiseAndSetIfChanged(ref _credentialType, value);
        }

        private string _credentialImageUrl;
        public string CredentialImageUrl
        {
            get => _credentialImageUrl;
            set => this.RaiseAndSetIfChanged(ref _credentialImageUrl, value);
        }

        private string _credentialSubtitle;
        public string CredentialSubtitle
        {
            get => _credentialSubtitle;
            set => this.RaiseAndSetIfChanged(ref _credentialSubtitle, value);
        }

        private bool _isNew;
        public bool IsNew
        {
            get => _isNew;
            set => this.RaiseAndSetIfChanged(ref _isNew, value);
        }

        private string _qRImageUrl;
        public string QRImageUrl
        {
            get => _qRImageUrl;
            set => this.RaiseAndSetIfChanged(ref _qRImageUrl, value);
        }

        private RangeEnabledObservableCollection<SSICredentialAttribute> _attributes;
        public RangeEnabledObservableCollection<SSICredentialAttribute> Attributes
        {
            get => _attributes;
            set => this.RaiseAndSetIfChanged(ref _attributes, value);
        }

        #endregion
    }
}
