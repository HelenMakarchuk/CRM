using CRM.Data;
using CRM.Models;
using CRM.Views.UserView;
using Newtonsoft.Json;
using System;
using System.Globalization;
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

            if (CurrentUser.FullName != String.Empty && CurrentUser.FullName != null)
            {
                FullNameLabel.Text += CurrentUser.FullName;
                FullNameLabel.IsVisible = true;
            }

            if (CurrentUser.Email != String.Empty && CurrentUser.Email != null)
            {
                EmailLabel.Text += CurrentUser.Email;
                EmailLabel.IsVisible = true;
            }

            if (CurrentUser.Phone != String.Empty && CurrentUser.Phone != null)
            {
                PhoneLabel.Text += CurrentUser.Phone;
                PhoneLabel.IsVisible = true;
            }

            if (CurrentUser.Position != String.Empty && CurrentUser.Position != null)
            {
                PositionLabel.Text += CurrentUser.Position;
                PositionLabel.IsVisible = true;
            }

            if (CurrentUser.Login != String.Empty && CurrentUser.Login != null)
            {
                LoginLabel.Text += CurrentUser.Login;
                LoginLabel.IsVisible = true;
            }

            if (CurrentUser.Password != String.Empty && CurrentUser.Password != null)
            {
                PasswordLabel.Text += CurrentUser.Password;
                PasswordLabel.IsVisible = true;
            }

            if (PickerData.genders.ContainsKey(user.Gender ?? ""))
            {
                GenderLabel.Text += PickerData.genders[user.Gender];
                GenderLabel.IsVisible = true;
            }

            if (user.BirthDate != null)
            {
                BirthDateLabel.Text += ((DateTime)user.BirthDate).ToString("d MMMM yyyy", new CultureInfo("en-US"));
                BirthDateLabel.IsVisible = true;
            }

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

                        if (department.Name != String.Empty && department.Name != null)
                        {
                            DepartmentLabel.Text += department.Name;
                            DepartmentLabel.IsVisible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", ex.Message, "OK");
                    }
                }
                else
                {
                    await DisplayAlert("response.StatusCode", response.StatusCode.ToString(), "OK");
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