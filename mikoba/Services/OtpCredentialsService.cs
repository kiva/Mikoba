using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace mikoba.Services
{
    public class OtpCredentialsService
    {
        private const string AUTH0_DOMAIN = "kiva-protocol.auth0.com";
        private const string CLIENT_ID = "dn2GNhnpOViDG1P1prNAt5UBoN6YEEHu";

        private const string USERNAME = "";
        private const string PASSWORD = "";
        
        // ReSharper disable once UnusedMember.Global
        public async Task<TokenResponse> GetAccessInfo()
        {
            var endpoint = AUTH0_DOMAIN + "/oauth/token";
            var method = "POST";
            var json = JsonConvert.SerializeObject(new
            {
                username = USERNAME,
                password = PASSWORD,
                client_id = CLIENT_ID,
                grant_type = "password",
            });
            var wc = new WebClient {Headers = {["Content-Type"] = "application/json"}};
            try
            {
                var response =await wc.UploadStringTaskAsync(endpoint, method, json);
                var userResult = JsonConvert.DeserializeObject<TokenResponse>(response);
                return userResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        
        // ReSharper disable once UnusedMember.Global
        public async Task<OtpResponse> RequestOTPCode(string nin, string phoneNumber)
        {
            var endpoint = AUTH0_DOMAIN + "/oauth/token";
            var method = "POST";
            var json = JsonConvert.SerializeObject(new
            {
                username = USERNAME,
                password = PASSWORD,
                client_id = CLIENT_ID,
                grant_type = "password",
            });
            var wc = new WebClient {Headers = {["Content-Type"] = "application/json"}};
            try
            {
                var response = await wc.UploadStringTaskAsync(endpoint, method, json);
                var userResult = JsonConvert.DeserializeObject<OtpResponse>(response);
                return userResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        
        public async Task<KycResponse> FetchKYCData(string otpNumber)
        {
            var endpoint = AUTH0_DOMAIN + "/oauth/token";
            var method = "POST";
            var json = JsonConvert.SerializeObject(new
            {
                username = USERNAME,
                password = PASSWORD,
                client_id = CLIENT_ID,
                grant_type = "password",
            });
            var wc = new WebClient {Headers = {["Content-Type"] = "application/json"}};
            try
            {
                var response = await wc.UploadStringTaskAsync(endpoint, method, json);
                var userResult = JsonConvert.DeserializeObject<KycResponse>(response);
                return userResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
