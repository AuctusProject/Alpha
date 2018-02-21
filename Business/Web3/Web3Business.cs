using Auctus.DomainObjects.Web3;
using Auctus.Util.NotShared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Auctus.Business.Web3
{
    public class Web3Business
    {
        public static Transaction CheckTransaction(string transactionHash)
        {
            return Get<Transaction>($"api/v1/transaction/{transactionHash}", 429, 404);
        }

        public static Transaction FaucetTransaction(string address)
        {
            return Post<Transaction>("api/v1/faucet", new { address }, 429);
        }

        public static Transaction MakeEscrowResultTransaction(string from, string to, double value)
        {
            return Post<Transaction>("api/v1/escrowresult", new { from , to, value }, 429);
        }

        private static T Post<T>(string route, object contentObject, params int[] expectedErrors)
        {
            using (var client = CreateWeb3Client())
            {
                var content = contentObject != null ? new StringContent(JsonConvert.SerializeObject(contentObject), Encoding.UTF8, "application/json") : null;
                using (HttpResponseMessage response = client.PostAsync(route, content).Result)
                {
                    return HandleResponse<T>(response, expectedErrors);
                }
            }
        }

        private static T Get<T>(string route, params int[] expectedErrors)
        {
            using (var client = CreateWeb3Client())
            {
                using (HttpResponseMessage response = client.GetAsync(route).Result)
                {
                    return HandleResponse<T>(response, expectedErrors);
                }
            }
        }

        private static T HandleResponse<T>(HttpResponseMessage response, params int[] expectedErrors)
        {
            var responseContent = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(responseContent);
            else if (expectedErrors != null && expectedErrors.Contains((int)response.StatusCode))
                throw new Web3Exception((int)response.StatusCode, JsonConvert.DeserializeObject<Error>(responseContent).Message);
            else
                throw new Exception(responseContent);
        }

        private static HttpClient CreateWeb3Client()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Config.WEB3_URL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
