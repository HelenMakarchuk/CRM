using CRM.Data;
using CRM.Models;
using CRM.Views.UserView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        User CurrentUser { get; set; }

        public UserPage()
        {
            InitializeComponent();
        }

        public UserPage(User user)
        {
            InitializeComponent();

            CurrentUser = user;

            EditToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/data_edit.png" : "data_edit.png";
            DeleteToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/garbage_closed.png" : "garbage_closed.png";

            FullNameEntry.Text = CurrentUser.FullName;
            EmailEntry.Text = CurrentUser.Email;
            PhoneEntry.Text = CurrentUser.Phone;
            PositionEntry.Text = CurrentUser.Position;
            LoginEntry.Text = CurrentUser.Login;
            PasswordEntry.Text = CurrentUser.Password;

            GenderPicker.ItemsSource = PickerData.genders.Values.ToList();
            GenderPicker.SelectedItem = PickerData.genders.ContainsKey(user.Gender ?? "") ? PickerData.genders[user.Gender] : null;

            BirthDatePicker.Date = user.BirthDate ?? DateTime.MinValue;

            SetUserDepartment();
        }

        protected async void SetUserDepartment()
        {
            if (CurrentUser.DepartmentId != null)
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Department.PluralDbTableName}/{CurrentUser.DepartmentId}"),
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
                        Department department = JsonConvert.DeserializeObject<Department>(json);
                        DepartmentPicker.ItemsSource = new List<Department>() { department };
                        DepartmentPicker.SelectedItem = department;

                        if (department.Name == String.Empty || department.Name == null)
                            DepartmentPicker.Title = "";
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", ex.Message, "OK");
                        DepartmentPicker.ItemsSource = new List<Department>();
                    }
                }
                else
                {
                    await DisplayAlert("response.StatusCode", response.StatusCode.ToString(), "OK");
                    DepartmentPicker.ItemsSource = new List<Department>();
                }
            }
        }

        void Edit_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new EditableUserPage(CurrentUser));
            }
            catch (Exception ex)
            {
                DisplayAlert("Update operation", $"User wasn't updated. {ex.Message}", "OK");
            }
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            var userResponse = await DisplayAlert("Are you sure?", "An item will be deleted", "Yes", "No");

            if (userResponse)
            {
                var uri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}/{CurrentUser.Id}");

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