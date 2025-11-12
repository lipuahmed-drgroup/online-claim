using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using RI.Claim.Entity;
using Newtonsoft.Json.Linq;

//using System.Collections.Generic;
//using System.Linq;

namespace RI.Claim.Api
{
    class ClaimApi
    {
        public void ReadData()
        {
            RiskVHAEntities db = new RiskVHAEntities();
        }

        private async Task<string> GetResponse(string request)
        {
            string _Url = "";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient client = new HttpClient();
            var data = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_Url),
                Content = new StringContent(request),
            };
            var response = await client.SendAsync(data);
            var responseInfo = await response.Content.ReadAsStringAsync();
            return responseInfo;
        }

        public static JObject GetResponse1(string jsontosend)
        {
            try
            {
                string apiURL = "";

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpWebRequest httpWR = (HttpWebRequest)WebRequest.Create(apiURL);
                httpWR.Credentials = CredentialCache.DefaultCredentials;
                httpWR.ContentType = "text/json";
                httpWR.Method = "POST";
                var streamWE = new StreamWriter(httpWR.GetRequestStream());
                streamWE.Write(jsontosend);
                streamWE.Flush();
                streamWE.Close();
                var httpRE = (HttpWebResponse)httpWR.GetResponse();
                var streamRE = new StreamReader(httpRE.GetResponseStream());
                return JObject.Parse(streamRE.ReadToEnd());
            }
            catch (Exception ex)
            {
                //log.Error("Rex error: " + ex);
                return null;
            }
        }

    }
}
