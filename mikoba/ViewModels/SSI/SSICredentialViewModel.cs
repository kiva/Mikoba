using System.Collections.Generic;
using Hyperledger.Aries.Features.IssueCredential;
using mikoba.Extensions;
using mikoba.Services;
using ReactiveUI;
using Xamarin.Forms.Xaml;

namespace mikoba.ViewModels.SSI
{
    public class SSICredentialViewModel : KivaBaseViewModel
    {
        public SSICredentialViewModel(
            INavigationService navigationService,
            CredentialRecord credential
        ) : base(
            nameof(SSICredentialViewModel),
            navigationService
        )
        {
            _credential = credential;
            Attributes = new RangeEnabledObservableCollection<SSICredentialAttribute>();
            foreach (var cred in _credential.CredentialAttributesValues)
            {
                Attributes.Add(new SSICredentialAttribute()
                {
                    Name = cred.Name,
                    Value = cred.Value,
                });
            }
            
#if DEBUG
            _credentialName = "Credential Name";
            
            _credentialSubtitle = "10/22/2017";
            _credentialType = "Bank Statement";
            
#endif
            _credentialName = "Civil Registry Office";
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
