using CRM.Data;
using CRM.Models;
using CRM.Views.UserView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Current user deleting
        /// </summary>
        async void Delete()
        {
            try
            {
                var uri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}/{CurrentUser.Id}");

                var client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Delete operation", $"User wasn't deleted. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Delete operation", $"User wasn't deleted. {ex.Message}", "OK");
            }
        }

        async void CheckRelatedDepartments()
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Department.PluralDbTableName}/$HeadId={CurrentUser.Id}"),
                    Method = HttpMethod.Get,
                    Headers = { { "Accept", "application/json" } }
                };

                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent content = response.Content;
                    string json = await content.ReadAsStringAsync();

                    List<string> departmentNames = JsonConvert.DeserializeObject<List<string>>(json);

                    if (departmentNames.Count > 0)
                    {
                        await DisplayAlert("Deleting is not allowed", $"The user is linked to departments: {String.Join(", ", departmentNames)}", "OK");
                        return;
                    }

                    Delete();
                }
                else
                {
                    await DisplayAlert("Retrieve related departments operation", $"Response status: {response.StatusCode}", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Retrieve related departments operation", $"An error occured while retrieving related departments. {ex.Message}", "OK");
                return;
            }
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            var userResponse = await DisplayAlert("Are you sure?", "An item will be deleted", "Yes", "No");

            if (userResponse)
            {
                try
                {
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{Order.PluralDbTableName}/$OwnerId={CurrentUser.Id}||$DeliveryDriverId={CurrentUser.Id}"),
                        Method = HttpMethod.Get,
                        Headers = { { "Accept", "application/json" } }
                    };

                    var client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HttpContent content = response.Content;
                        string json = await content.ReadAsStringAsync();

                        List<string> orderNumbers = JsonConvert.DeserializeObject<List<string>>(json);
                        
                        if (orderNumbers.Count > 0)
                        {
                            await DisplayAlert("Deleting is not allowed", $"The user is linked to orders: {String.Join(", ", orderNumbers)}", "OK");
                            return;
                        }

                        CheckRelatedDepartments();
                    }
                    else
                    {
                        await DisplayAlert("Retrieve related orders operation", $"Response status: {response.StatusCode}", "OK");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Retrieve related orders operation", $"An error occured while retrieving related orders. {ex.Message}", "OK");
                    return;
                }
            }
        }
    }
}