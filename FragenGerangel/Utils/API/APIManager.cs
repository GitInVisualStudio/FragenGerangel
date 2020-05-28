using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace FragenGerangel.Utils.API
{
    public class APIManager
    {
        private static readonly HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("http://127.0.0.1:80/api/")
        };

        private string username;
        private string password;
        private string auth;
        private DateTime authSince;

        public APIManager(string username, string password)
        {
            this.username = username;
            this.password = password;

            Init().Wait();
        }

        private async Task Init()
        {
            auth = await GetAuthToken().ConfigureAwait(false);
            authSince = DateTime.Now;
        }

        private async Task<string> Post(string uri, JObject json)
        {
            StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, uri);
            message.Content = content;
            HttpResponseMessage response = await client.SendAsync(message).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async Task<JObject> PostReturnJson(string uri, JObject json)
        {
            if (uri != "getAuthToken.php" && DateTime.Now - authSince >= new TimeSpan(24, 0, 0))
                Init().Wait();
            string resultStr = await Post(uri, json).ConfigureAwait(false);
            Console.WriteLine(resultStr);
            JObject resultJson = JObject.Parse(resultStr);
            if (resultJson["result"].ToString() != "ok")
                throw APIExceptionManager.FromID(resultJson["error_code"].ToObject<int>());
            return resultJson;
        }

        private async Task<string> GetAuthToken()
        {
            JObject json = new JObject();
            json["username"] = username;
            json["password"] = password;
            JObject resultJson = await PostReturnJson("getAuthToken.php", json).ConfigureAwait(false);
            return resultJson["token"].ToString();
        }
    }
}
