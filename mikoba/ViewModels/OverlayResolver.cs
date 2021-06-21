using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AppCenter.Crashes;
using mikoba.ViewModels.SSI;
using Newtonsoft.Json;
using Sentry;

namespace mikoba.ViewModels
{
    public static class OverlayResolver
    {
        
       public static CredentialOverlay GetCredentialOverlay()
        {
            var result = new CredentialOverlay();
            result.Fields = new Dictionary<string, CredentialOverlayField>()
            {
                {"firstName", new CredentialOverlayField() {
                   Properties = new Dictionary<string, string>()
                   {
                       {"name", "First Name"},
                       {"dataType", "text"}
                   }
                }},
                {"lastName", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"name", "Last Name"},
                        {"dataType", "text"}
                    }
                }},
                {"companyEmail", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"name", "Company Email"},
                        {"dataType", "text"}
                    }
                }},
                {"currentTitle", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"name", "Current Title"},
                        {"dataType", "text"}
                    }
                }},
                {"team", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"name", "Team"},
                        {"dataType", "text"}
                    }
                }},
                {"hireDate", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"name", "Hire Date"},
                        {"dataType", "date"}
                    }
                }},
                {"officeLocation", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"name", "Office Location"},
                        {"dataType", "text"}
                    }
                }},
                {"phoneNumber", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"name", "Phone Number"},
                        {"dataType", "text"}
                    }
                }},
                {"photo~attach", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"name", "Photo"},
                        {"dataType", "image/jpeg;base64"}
                    }
                }},
                {"type", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"name", "Type"},
                        {"dataType", "selection"}
                    }
                }},
                {"endDate", new CredentialOverlayField() {
                    Properties = new Dictionary<string, string>()
                    {
                        {"name", "End Date"},
                        {"dataType", "date"}
                    }
                }}
            };
            /*
            var assembly = typeof(SSICredentialViewModel).GetTypeInfo().Assembly;

            foreach (var res in assembly.GetManifestResourceNames())
            {
                if (res.Contains("CredentialMapping.json"))
                {
                    CredentialDisplayNamesJsonString = "";
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
*/
            return result;
        }
        
  
    }
}