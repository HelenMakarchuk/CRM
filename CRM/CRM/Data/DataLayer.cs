using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CRM.Models;
using Newtonsoft.Json;

namespace CRM.Data
{
    public class DataLayer
    {
        public static DataLayer Instance { get; } = new DataLayer();

        public async Task<List<T>> GetDataAsync<T>()
        {
            var list = new List<T>();

            //every db table class contains static property named PluralDbTableName (Example: "Customer" => "Customers")
            var pluralDbTableName = typeof(T).GetProperty("PluralDbTableName").GetValue(null);

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{pluralDbTableName}"),
                Method = HttpMethod.Get,
                Headers = { { "Accept", "application/json" } }
            };

            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                string json = await content.ReadAsStringAsync();

                try
                {
                    list = JsonConvert.DeserializeObject<List<T>>(json);
                }
                catch (Exception)
                {
                    return list;
                }
            }

            return list;
        }
    }
}
