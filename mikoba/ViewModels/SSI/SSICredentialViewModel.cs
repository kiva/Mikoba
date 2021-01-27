using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Hyperledger.Aries.Features.IssueCredential;
using mikoba.Extensions;
using mikoba.Services;
using ReactiveUI;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Sentry;
using Microsoft.AppCenter.Crashes;

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
        
        private static string _credentialDisplayNamesJsonString;
        public static string CredentialDisplayNamesJsonString
        {
            get
            {
                return _credentialDisplayNamesJsonString;
            }
            set
            {
                _credentialDisplayNamesJsonString = value;
            }
        }
        
        private static string _credentialDisplayNames;
        public static string CredentialDisplayNames
        {
            get
            {
                return _credentialDisplayNames;
            }
            set
            {
                _credentialDisplayNames = value;
            }
        }



        class CredentialOverlay
        {
            public string CredentialId;
            public Dictionary<string,CredentialOverlayField> Fields { get; set; }
        }

        class CredentialOverlayField
        {
            public string Name { get; set; }
            public Dictionary<string,string> Properties { get; set; }

            public string DisplayName
            {
                get
                {
                    return Properties["displayName"];
                }
            }
            
            public string DataType
            {
                get
                {
                    return Properties["dataType"];
                }
            }
        }

        private static CredentialOverlay GetCredentialOverlay()
        {
            var result = new  CredentialOverlay();
            result.Fields = new Dictionary<string, CredentialOverlayField>()
            {
                {"firstName", new CredentialOverlayField() {
                   Properties = new Dictionary<string, string>()
                   {
                       {"displayName", "First Name"},
                       {"dataType", "text"}
                   }
                }},
                {"lastName", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"displayName", "Last Name"},
                        {"dataType", "lastName"}
                    }
                }},
                {"companyEmail", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"displayName", "Company Email"},
                        {"dataType", "companyEmail"}
                    }
                }},
                {"currentTitle", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"displayName", "Current Title"},
                        {"dataType", "currentTitle"}
                    }
                }},
                {"team", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"displayName", "Team"},
                        {"dataType", "team"}
                    }
                }},
                {"hireDate", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"displayName", "Hire Date"},
                        {"dataType", "hireDate"}
                    }
                }},
                {"officeLocation", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"displayName", "Office Location"},
                        {"dataType", "officeLocation"}
                    }
                }},
                {"phoneNumber", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"displayName", "Phone Number"},
                        {"dataType", "phoneNumber"}
                    }
                }},
                {"photo~attach", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"displayName", "Photo"},
                        {"dataType", "image/jpeg;base64"}
                    }
                }},
                {"type", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"displayName", "Type"},
                        {"dataType", "selection"}
                    }
                }},
                {"endDate", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"displayName", "End Date"},
                        {"dataType", "date"}
                    }
                }}
            };

            return result;
            //TODO: Finish serialization.
            var assembly = typeof(SSICredentialViewModel).GetTypeInfo().Assembly;

            foreach (var res in assembly.GetManifestResourceNames())
            {
                if (res.Contains("CredentialMapping.json"))
                {
                    Stream stream = assembly.GetManifestResourceStream(res);

                    using (var reader = new StreamReader(stream))
                    {
                        string data = "";
                        while ((data = reader.ReadLine()) != null)
                        {
                            CredentialDisplayNamesJsonString = CredentialDisplayNamesJsonString + data;
                        }

                        try
                        {
                            var CredentialDisplayNames = JsonConvert.DeserializeObject<CredentialOverlay>(CredentialDisplayNamesJsonString);
                        }
                        catch (Exception ex)
                        {
                            Crashes.TrackError(ex);
                            SentrySdk.CaptureException(ex);
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }

        public static string FormatCredentialName(string source)
        {
            var overlay = GetCredentialOverlay();
            if (overlay.Fields.ContainsKey(source))
            {
                return overlay.Fields[source].DisplayName;
            }

            return source;
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
