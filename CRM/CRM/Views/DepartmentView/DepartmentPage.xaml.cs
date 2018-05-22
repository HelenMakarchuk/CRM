using CRM.Data;
using CRM.Models;
using CRM.Views.UserView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views.DepartmentView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DepartmentPage : ContentPage
    {
        Department CurrentDepartment { get; set; }

        public DepartmentPage()
        {
            InitializeComponent();
        }

        public DepartmentPage(Department department)
        {
            InitializeComponent();

            CurrentDepartment = department;

            EditToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/data_edit.png" : "data_edit.png";
            DeleteToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/garbage_closed.png" : "garbage_closed.png";

            if (CurrentDepartment.Name != String.Empty && CurrentDepartment.Name != null)
            {
                NameLabel.Text += CurrentDepartment.Name;
                NameLabel.IsVisible = true;
            }

            if (CurrentDepartment.Phone != String.Empty && CurrentDepartment.Phone != null)
            {
                PhoneLabel.Text += CurrentDepartment.Phone;
                PhoneLabel.IsVisible = true;
            }

            SetDepartmentHead();
        }

        protected async void SetDepartmentHead()
        {
            if (CurrentDepartment.HeadId != null)
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}/{CurrentDepartment.HeadId}"),
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
                        User head = JsonConvert.DeserializeObject<User>(json);

                        if (head.FullName != String.Empty && head.FullName != null)
                        {
                            HeadLabel.Text += head.FullName;
                            HeadLabel.IsVisible = true;
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

        async void UsersButton_Clicked()
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}/$DepartmentId={CurrentDepartment.Id}"),
                    Method = HttpMethod.Get,
                    Headers = { { "Accept", "application/json" } }
                };

                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent content = response.Content;
                    string json = await content.ReadAsStringAsync();

                    List<User> users = JsonConvert.DeserializeObject<List<User>>(json);

                    if (users.Count > 0)
                    {
                        await Navigation.PushAsync(new DepartmentUsers(CurrentDepartment));
                    }
                    else
                    {
                        await DisplayAlert("Retrieving users operation", $"The department is linked to no one user", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Retrieving users operation", $"Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Retrieving users operation", $"An error occured while retrieving department users. {ex.Message}", "OK");
            }
        }

        void Edit_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new EditableDepartmentPage(CurrentDepartment));
            }
            catch(Exception ex)
            {
                DisplayAlert("Update operation", $"Department wasn't updated. {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Current department deleting
        /// </summary>
        async void Delete()
        {
            try
            {
                var uri = new Uri($"{Constants.WebAPIUrl}/api/{Department.PluralDbTableName}/{CurrentDepartment.Id}");

                var client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Delete operation", $"Department wasn't deleted. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Delete operation", $"Department wasn't deleted. {ex.Message}", "OK");
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
                        RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}" +
                                $"/$DepartmentId={CurrentDepartment.Id}" +
                                $"/$select=FullName"),

                        Method = HttpMethod.Get,
                        Headers = { { "Accept", "application/json" } }
                    };

                    var client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HttpContent content = response.Content;
                        string json = await content.ReadAsStringAsync();

                        List<string> userNames = JsonConvert.DeserializeObject<List<string>>(json);

                        if (userNames.Count > 0)
                        {
                            await DisplayAlert("Deleting is not allowed", $"The department is linked to users: {String.Join(", ", userNames)}", "OK");
                            return;
                        }

                        Delete();
                    }
                    else
                    {
                        await DisplayAlert("Retrieve related users operation", $"Response status: {response.StatusCode}", "OK");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Retrieve related users operation", $"An error occured while retrieving related users. {ex.Message}", "OK");
                    return;
                }
            }
        }
    }
}