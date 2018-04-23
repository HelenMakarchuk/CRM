using CRM.Data;
using CRM.Models;
using CRM.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewDepartmentPage : ContentPage
    {
        public NewDepartmentPage()
        {
            InitializeComponent();

            SaveToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/save.png" : "save.png";

            FillHeadPicker();
            BindingContext = this;
        }

        protected async void FillHeadPicker()
        {
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
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
                    users = users.Select(u => { u.FullName = (u.FullName ?? ""); return u; }).ToList();

                    //Add empty value opportunity
                    users.Insert(0, new User() { FullName = "Empty value" });

                    HeadPicker.ItemsSource = users;
                }
                catch (Exception)
                {
                    HeadPicker.ItemsSource = new List<User>();
                }
            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                #region Checks

                if (NameEntry.Text == string.Empty || NameEntry.Text == null)
                {
                    await DisplayAlert("Create operation", "Name must be set", "OK");
                    return;
                }

                #endregion

                #region New department assembling

                Department department = new Department();
                department.Name = NameEntry.Text;

                if (PhoneEntry.Text != string.Empty)
                    department.Phone = PhoneEntry.Text;

                //if (HeadPicker.SelectedIndex == 0) { user want to leave head empty (null) } 
                if (HeadPicker.SelectedIndex != 0 && HeadPicker.SelectedItem is User selectedHead)
                {
                    department.HeadId = selectedHead.Id;
                }

                #endregion

                string json = JsonConvert.SerializeObject(department);
                var content = new StringContent(json);

                var client = new HttpClient();
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"{Constants.WebAPIUrl}/api/{Department.PluralDbTableName}", content);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    //var newDepartmentUri = response.Headers.Location;

                    await DisplayAlert("Create operation", $"Department \"{NameEntry.Text}\" was created.", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Create operation", $"Department \"{NameEntry.Text}\" wasn't created. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Create operation", $"An error occured while creating department \"{NameEntry.Text}\". {ex.Message}", "OK");
            }
        }
    }
}