using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ActiveDirectorySearch
{
    public class NXOperation 
    {
      
        public string NXOperationUrl { get; private set; }
        private HttpClient client;
        JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
        private string AuthorizationHeader
        {
            get
            {
                string UnParseString = $"{ADConfig.AgilePointUsername}:{ADConfig.AgilePointPassword}";
                string ParsedString = Convert.ToBase64String(Encoding.ASCII.GetBytes(UnParseString));
                return ParsedString;
            }
        }

        public string GetRegisterUser(string Username)
        {
            string URI = $"{ADConfig.AgilePointServiceBaseUrl}/Admin/GetRegisterUser";

            string jsonData = "{\"userName\":\"" + Username + "\"}";

            string response = HttpPost(URI, ADConfig.AgilePointUsername, jsonData).Result;
            return response;

            
        }
        public void RegisterUser( NXUserProfile UserProfile)
        {

            if (!isUserExistsInAgilepoint(UserProfile.UserName))
            {
                string URI = $"{ADConfig.AgilePointServiceBaseUrl}/Admin/RegisterUser";

                string jsonData = jsonConvert.Serialize(UserProfile);

                string response = HttpPost(URI, ADConfig.AgilePointUsername, jsonData).Result;
                Logger.Info($"{ UserProfile.UserName} registration is succesful in Agilepoint");
            }
        }

        private bool isUserExistsInAgilepoint(string userName)
        {
           bool isUserExists = false;
           string response= GetRegisterUser(userName);
            NXUserProfile UserProfile=   jsonConvert.Deserialize<NXUserProfile>(response);
            
            if (!string.IsNullOrWhiteSpace(UserProfile?.UserName))
            {
                isUserExists = true;
                Logger.Info($"{ userName} is alredy registered user in Agilepoint");
            }
            return isUserExists;
        }

        protected async Task<string> HttpGet(string url, string authUserName)
        {
            client = new HttpClient
            {
                Timeout = new TimeSpan(0, 0, 30)
            };
            try
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {AuthorizationHeader}");
                client.DefaultRequestHeaders.Add("authUserName", authUserName);
               client.DefaultRequestHeaders.Add("appID", ADConfig.AppName);
                HttpResponseMessage result = await client.GetAsync(url).ConfigureAwait(false);
                string response = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!result.IsSuccessStatusCode)
                {
                    Logger.Error($"Exception occurred while performing NX Operation '{url}' , Error response : {response} !");
                }
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred while performing NX Operation '{url}' , Exception : {ex.Message} !");
                return null;
            }
        }

        protected async Task<string> HttpPost(string url, string authUserName, string data = null)
        {
            client = new HttpClient
            {
                Timeout = new TimeSpan(0, 0, 30)
            };
            try
            {
                // if no data for payload replace using new object or {}
                using (StringContent content = new StringContent(data ?? "{}", Encoding.UTF8))
                {
                    content.Headers.Remove("Content-Type");
                    content.Headers.Add("Content-Type", "application/json");
                    client.DefaultRequestHeaders.Add("Authorization", $"Basic {AuthorizationHeader}");
                    client.DefaultRequestHeaders.Add("authUserName", authUserName);
                   client.DefaultRequestHeaders.Add("appID", ADConfig.AppName);
                    HttpResponseMessage result = await client.PostAsync(url, content).ConfigureAwait(false);
                    string response = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!result.IsSuccessStatusCode)
                    {
                        Logger.Error($"Exception occurred while performing NX Operation '{url}' , Error response : {response} !");
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception occurred while performing NX Operation '{url}' , Exception : {ex.Message}!");
                throw;
            }
        }

        public virtual void Dispose()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }
    }
}
