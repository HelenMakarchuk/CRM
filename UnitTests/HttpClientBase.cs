using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class HttpClientBase : HttpClient
    {
        public async Task<List<T>> GetListItems<T>()
        {
            //every dbTableClass contains static property named PluralDbTableName (Class "Customer" => "Customers")
            var pluralDbTableName = typeof(T).GetProperty("PluralDbTableName").GetValue(null);

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{pluralDbTableName}"),
                Method = HttpMethod.Get,
                Headers = { { "Accept", "application/json" } }
            };

            //var request = new HttpRequestMessage();
            //request.RequestUri = new Uri($"{Constants.WebAPIUrl}/api/Customers");
            //request.Method = HttpMethod.Get;
            //request.Headers.Add("Accept", "application/json");

            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK) { }

            HttpContent content = response.Content;
            string json = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        public async Task<List<String>> GetListItems1<T>()
        {
            var pluralDbTableName = "values";

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{pluralDbTableName}");

            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");

            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK) { }

            HttpContent content = response.Content;
            string contentString = await content.ReadAsStringAsync();

            var json = JsonConvert.DeserializeObject<List<String>>(contentString);

            //int y = 1;

            return json;



            //return new List<string>() { contentString, "dsfsdf", "dfs" };












            //var response = await GetAsync(uri);

            //if (response.IsSuccessStatusCode)
            //{
            //    var content = await response.Content.ReadAsStringAsync();
            //    var Items = JsonConvert.DeserializeObject<List<T>>(content);

            //    return Items;
            //}

            //throw new Exception(response.ReasonPhrase);

            //var pluralDbTableName = typeof(T).GetProperty("PluralDbTableName").GetValue(null);
            //var uri = new Uri($"{Constants.WebAPIUrl}/api/{pluralDbTableName}");

            //var response = await GetAsync(uri);

            //if (response.IsSuccessStatusCode)
            //{
            //    var content = await response.Content.ReadAsStringAsync();
            //    var Items = JsonConvert.DeserializeObject<List<T>>(content);

            //    return Items;
            //}

            //throw new Exception(response.ReasonPhrase);
        }

        public async Task<T> GetItem<T>(Guid Id)
        {
            var pluralDbTableName = typeof(T).GetProperty("PluralDbTableName").GetValue(null);
            var uri = new Uri($"{Constants.WebAPIUrl}/api/{pluralDbTableName}/{Id}");
            var response = await GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Item = JsonConvert.DeserializeObject<T>(content);
                return Item;
            }
            throw new Exception(response.ReasonPhrase);
        }

        public async Task PostItem<T>(T item)
        {
            var pluralDbTableName = typeof(T).GetProperty("PluralDbTableName").GetValue(null);
            var uri = new Uri($"{Constants.WebAPIUrl}/api/{pluralDbTableName}/{item.GetType().GetProperty("Id")}");
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;
            response = await PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            throw new Exception(response.ReasonPhrase);
        }
    }
}
