using CRM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
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

            NameEntry.Text = CurrentDepartment.Name;
            PhoneEntry.Text = CurrentDepartment.Phone;
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
                        HeadPicker.ItemsSource = new List<User>() { head };
                        HeadPicker.SelectedItem = head;

                        if (head.FullName == String.Empty || head.FullName == null)
                            HeadPicker.Title = "";
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", ex.Message, "OK");
                        HeadPicker.ItemsSource = new List<User>();
                    }
                }
                else
                {
                    await DisplayAlert("response.StatusCode", response.StatusCode.ToString(), "OK");
                    HeadPicker.ItemsSource = new List<User>();
                }
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

        async void Delete_Clicked(object sender, EventArgs e)
        {
            var userResponse = await DisplayAlert("Are you sure?", "An item will be deleted", "Yes", "No");

            if (userResponse)
            {
                var uri = new Uri($"{Constants.WebAPIUrl}/api/{Department.PluralDbTableName}/{CurrentDepartment.Id}");

                var client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await DisplayAlert("Delete operation", "Department was deleted", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Delete operation", $"Department wasn't deleted. Response status: {response.StatusCode}", "OK");
                }
            }
        }
    }
}