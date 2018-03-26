using CRM.Data;
using CRM.Models;
using CRM.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        User currentUser { get; set; }

        public UserPage()
        {
            InitializeComponent();
        }

        public UserPage(User user)
        {
            InitializeComponent();

            currentUser = user;

            EditToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/data_edit.png" : "data_edit.png";
            DeleteToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/garbage_closed.png" : "garbage_closed.png";
            CloseToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/close.png" : "close.png";

            fullNameEntry.Text = user.FullName;
            phoneEntry.Text = user.Phone;
            emailEntry.Text = user.Email;
            positionEntry.Text = user.Position;
            loginEntry.Text = user.Login;
            passwordEntry.Text = user.Password;
            
            genderPicker.ItemsSource = PickerData.Genders.Values.ToList();
            genderPicker.SelectedItem = PickerData.Genders.ContainsKey(user.Gender ?? "") ? PickerData.Genders[user.Gender] : null;

            SetUserDepartment(user);

            birthDatePicker.Date = user.BirthDate ?? DateTime.MinValue;
        }

        protected async void SetUserDepartment(User user)
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
                    departmentPicker.ItemsSource = departments;
                    departmentPicker.SelectedItem = departments.Where(d => d.Id == user.DepartmentId).FirstOrDefault();
                }
                catch (Exception)
                {
                    departmentPicker.ItemsSource = new List<string>();
                }
            }
        }

        async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void Edit_Clicked(object sender, EventArgs e)
        {
            try
            {
                var userResponse = await DisplayAlert("Are you sure?", "An item will be updated", "Yes", "No");

                if (userResponse)
                {
                    User updUser = new User();
                    updUser.Id = currentUser.Id;
                    updUser.FullName = fullNameEntry.Text;
                    updUser.Login = loginEntry.Text;
                    updUser.Password = passwordEntry.Text;
                    updUser.Phone = phoneEntry.Text;
                    updUser.Position = positionEntry.Text;
                    updUser.BirthDate = birthDatePicker.Date;
                    updUser.Email = emailEntry.Text;
                    updUser.DepartmentId = (departmentPicker.SelectedItem as Department).Id;
                    updUser.Gender = genderPicker.SelectedItem?.ToString().First().ToString();

                    var json = JsonConvert.SerializeObject(updUser);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var uri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}/{currentUser.Id}");
                    var client = new HttpClient();

                    HttpResponseMessage response = await client.PutAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Update operation", "User was updated", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Update operation", $"User wasn't updated. Response status: {response.StatusCode}", "OK");
                    }
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Update operation", $"User wasn't updated. {ex.Message}", "OK");
            }
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            var userResponse = await DisplayAlert("Are you sure?", "An item will be deleted", "Yes", "No");

            if (userResponse)
            {
                var uri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}/{currentUser.Id}");

                var client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await DisplayAlert("Delete operation", "User was deleted", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Delete operation", $"User wasn't deleted. Response status: {response.StatusCode}", "OK");
                }
            }
        }
    }
}