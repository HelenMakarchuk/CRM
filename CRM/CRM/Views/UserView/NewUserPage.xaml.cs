using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CRM.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using CRM.Data;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewUserPage : ContentPage
    {
        public NewUserPage()
        {
            InitializeComponent();

            SaveToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/save.png" : "save.png";
            CloseToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/close.png" : "close.png";

            genderPicker.ItemsSource = PickerData.Genders.Values.ToList();
            GetDepartments();
            BindingContext = this;
        }

        protected async void GetDepartments()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Department.PluralDbTableName}"),
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
                    List<Department> departments = JsonConvert.DeserializeObject<List<Department>>(json);
                    departmentPicker.ItemsSource = departments.Select(d => d.Name).ToList();
                }
                catch (Exception)
                {
                    departmentPicker.ItemsSource = new List<string>();
                }
            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                #region User assembling
                User user = new User();

                if (fullNameEntry.Text != string.Empty)
                    user.FullName = fullNameEntry.Text;

                if (emailEntry.Text != string.Empty)
                    user.Email = emailEntry.Text;

                if (positionEntry.Text != string.Empty)
                    user.Position = positionEntry.Text;

                if (phoneEntry.Text != string.Empty)
                    user.Phone = phoneEntry.Text;

                if (loginEntry.Text != string.Empty)
                    user.Login = loginEntry.Text;

                if (passwordEntry.Text != string.Empty)
                    user.Password = passwordEntry.Text;

                #endregion

                string json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json);

                var client = new HttpClient();
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}", content);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    //var newUserUri = response.Headers.Location;
                    
                    await DisplayAlert("Create operation", $"User \"{fullNameEntry.Text}\" was created.", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Create operation", $"User \"{fullNameEntry.Text}\" wasn't created. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Create operation", $"An error occured while creating user \"{fullNameEntry.Text}\". {ex.Message}", "OK");
            }
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}