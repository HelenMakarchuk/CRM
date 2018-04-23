using CRM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditableDepartmentPage : ContentPage
    {
        Department CurrentDepartment { get; set; }

        public EditableDepartmentPage()
        {
            InitializeComponent();
        }

        public EditableDepartmentPage(Department department)
        {
            InitializeComponent();

            CurrentDepartment = department;

            SaveToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/save.png" : "save.png";

            NameEntry.Text = department.Name;
            PhoneEntry.Text = department.Phone;
            SetDepartmentHead();
        }

        protected async void SetDepartmentHead()
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

                    var selected = HeadPicker.SelectedItem = users.Where(u => u.Id == CurrentDepartment.HeadId).FirstOrDefault();
                    if (selected != null && ((User)selected).FullName == String.Empty)
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

                #region Updated department assembling

                Department department = new Department();
                department.Id = CurrentDepartment.Id;
                department.Name = NameEntry.Text;
                department.Phone = PhoneEntry.Text;

                //if (HeadPicker.SelectedIndex == 0) { user want to set head empty (null) } 
                if (HeadPicker.SelectedIndex != 0 && HeadPicker.SelectedItem is User selectedHead)
                {
                    department.HeadId = selectedHead.Id;
                }
                else
                {
                    department.HeadId = null;
                }

                #endregion

                var json = JsonConvert.SerializeObject(department);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var uri = new Uri($"{Constants.WebAPIUrl}/api/{Department.PluralDbTableName}/{CurrentDepartment.Id}");
                var client = new HttpClient();

                HttpResponseMessage response = await client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Update operation", "Department was updated", "OK");

                    if (Navigation.NavigationStack.Count > 0)
                    {
                        int currentPageIndex = Navigation.NavigationStack.Count - 1;
                        Navigation.RemovePage(Navigation.NavigationStack[currentPageIndex - 1]);
                        await Navigation.PushAsync(new DepartmentPage(department));
                        Navigation.RemovePage(Navigation.NavigationStack[currentPageIndex - 1]);
                    }
                }
                else
                {
                    await DisplayAlert("Update operation", $"Department wasn't updated. Response status: {response.StatusCode}", "OK");
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Update operation", $"Department wasn't updated. {ex.Message}", "OK");
            }
        }
    }
}