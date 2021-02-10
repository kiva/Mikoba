using System;
using Newtonsoft.Json;

namespace mikoba.Tools
{
    public static class CredentialTools
    {
        public static byte[] ProcessJSONImageField(string input)
        {
            var imageJson = JsonConvert.DeserializeObject<JSONImageField>(input);
            if (imageJson.type == "image/png")
            {
                return Convert.FromBase64String(imageJson.data);    
            }
            return new byte[]{};
        }
    }
}