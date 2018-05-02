﻿using CRM.Models;
using Newtonsoft.Json;
using System;
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