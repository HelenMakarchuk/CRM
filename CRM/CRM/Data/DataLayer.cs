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

        public async Task<List<User>> GetUsersAsync()
        {
            var userList = new List<User>();

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}"),
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
                    userList = JsonConvert.DeserializeObject<List<User>>(json);

                    //UsersListView.ItemsSource = users.Select(u => u.FullName).ToList();
                }
                catch (Exception)
                {
                    return userList;

                    //UsersListView.ItemsSource = new List<string>();
                }
            }

            return userList;
        }
    }
}
