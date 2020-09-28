using System.Collections.Generic;
using Hyperledger.Aries.Features.IssueCredential;
using mikoba.Services;
using ReactiveUI;

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
            _attributes = new List<SSICredentialAttribute>();
            
#if DEBUG
            _credentialName = "Credential Name";
            _credentialImageUrl = "http://placekitten.com/g/200/200";
            _credentialSubtitle = "10/22/2017";
            _credentialType = "Bank Statement";
            _qRImageUrl = "http://placekitten.com/g/100/100";
#endif
            _credentialName = _credential.TypeName;
        }
        
        private readonly CredentialRecord _credential;
        
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

        private IEnumerable<SSICredentialAttribute> _attributes;
        public IEnumerable<SSICredentialAttribute> Attributes
        {
            get => _attributes;
            set => this.RaiseAndSetIfChanged(ref _attributes, value);
        }

        #endregion
    }
}
