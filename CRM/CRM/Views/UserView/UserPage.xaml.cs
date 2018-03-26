using CRM.Data;
using CRM.Models;
using CRM.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();
        }

        public UserPage(User user)
        {
            InitializeComponent();

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
            //save record - put request (id, user)

            await DisplayAlert("Edit operation", "Department was updated", "OK");

            await Navigation.PopAsync();
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            var userResponse = await DisplayAlert("Are you sure?", "An item will be deleted", "Yes", "No");

            if (userResponse)
            {
                //delete request (id)

                await DisplayAlert("Delete operation", "Department was deleted", "OK");

                await Navigation.PopAsync();
            }
        }
    }
}