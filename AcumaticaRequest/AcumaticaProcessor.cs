using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Json.Net;
using Newtonsoft.Json;
using AcumaticaRequest.Model;

namespace AcumaticaRequest
{
    public class AcumaticaProcessor : IDisposable
    {
        private const string siteURL = "https://hackathon.acumatica.com/delta/";
        private const string baseURL = siteURL + "entity/";

        private const string loginURL = baseURL + "auth/login";
        private const string logoutURL = baseURL + "auth/logout";

        private const string endpointName = "Default";
        private const string endpointVersion = "18.200.001";

        private const string endpointURL = baseURL + endpointName + "/" + endpointVersion + "/";


        private HttpClient client = new HttpClient();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    //client.Logout();// We should be logging out here but if we do after awhile another thread will throw a "not logged in" exception, session sharing issue? 
                }
                catch (Exception e)
                {
                }
                finally
                {
                    //client.Close();
                }
            }
        }

        private void Logout()
        {
            HttpResponseMessage response = client.PostAsync(logoutURL, null).Result;
        }

        public void Login(string username, string password, string company = "Company")
        {
            client.DefaultRequestHeaders.Accept.Clear();
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            var loginDetails = new StringContent(
                $"{{ \"name\" : \"{username}\",\"password\" : \"{password}\",\"company\" : \"{company}\"}}",
                Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(loginURL, loginDetails).Result;
        }

        public void UpdateContractUsageDetails()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string body = "{  \"ContractID\": {            \"value\": \"05SUPPORTP\"        },        \"UnbilledTransactions\":        [        	{        	\"InventoryID\": {            \"value\": \"Vacuum\"        },        \"Qty\": {                \"value\": 1            } }        	]}";

            var response = client.PutAsync(endpointURL + "/ContractUsage", new StringContent(body)).Result;

            //Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

            string content = response.Content.ReadAsStringAsync().Result;

            //var result = JsonConvert.DeserializeObject<ContractUsage>(content);
        }

    }
}
