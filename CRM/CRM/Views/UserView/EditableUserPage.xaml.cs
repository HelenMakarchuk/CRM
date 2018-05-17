using CRM.Data;
using CRM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views.UserView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditableUserPage : ContentPage
	{
        User CurrentUser { get; set; }

        public EditableUserPage()
        {
            InitializeComponent();
        }

        public EditableUserPage(User user)
        {
            InitializeComponent();

            CurrentUser = user;

            SaveToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/save.png" : "save.png";

            FullNameEntry.Text = CurrentUser.FullName;
            EmailEntry.Text = CurrentUser.Email;
            PhoneEntry.Text = CurrentUser.Phone;
            PositionEntry.Text = CurrentUser.Position;
            LoginEntry.Text = CurrentUser.Login;
            PasswordEntry.Text = CurrentUser.Password;

            GenderPicker.ItemsSource = UserPickerData.genders.Values.ToList();
            GenderPicker.SelectedItem = UserPickerData.genders.ContainsKey(user.Gender ?? "") ? UserPickerData.genders[user.Gender] : null;

            BirthDatePicker.Date = user.BirthDate ?? DateTime.MinValue;

            SetUserDepartment();
        }

        protected async void SetUserDepartment()
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
                    departments = departments.Select(d => { d.Name = (d.Name ?? ""); return d; }).ToList();

                    //Add empty value opportunity
                    departments.Insert(0, new Department() { Name = "Empty value" });

                    DepartmentPicker.ItemsSource = departments;

                    var selected = DepartmentPicker.SelectedItem = departments.Where(d => d.Id == CurrentUser.DepartmentId).FirstOrDefault();
                    if (selected != null && ((Department)selected).Name == String.Empty)
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

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                #region Updated user assembling

                User user = new User();
                user.Id = CurrentUser.Id;
                user.FullName = FullNameEntry.Text;
                user.Email = EmailEntry.Text;
                user.Phone = PhoneEntry.Text;
                user.Position = PositionEntry.Text;
                user.Login = LoginEntry.Text;
                user.Password = PasswordEntry.Text;
                user.Gender = GenderPicker.SelectedItem?.ToString().First().ToString();
                user.BirthDate = BirthDatePicker.Date;

                //if (DepartmentPicker.SelectedIndex == 0) { user want to set department empty (null) } 
                if (DepartmentPicker.SelectedIndex != 0 && DepartmentPicker.SelectedItem is Department selectedDepartment)
                {
                    user.DepartmentId = selectedDepartment.Id;
                }
                else
                {
                    user.DepartmentId = null;
                }

                #endregion

                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var uri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}/{CurrentUser.Id}");
                var client = new HttpClient();

                HttpResponseMessage response = await client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Update operation", "User was updated", "OK");

                    if (Navigation.NavigationStack.Count > 0)
                    {
                        int currentPageIndex = Navigation.NavigationStack.Count - 1;
                        Navigation.RemovePage(Navigation.NavigationStack[currentPageIndex - 1]);
                        await Navigation.PushAsync(new UserPage(user));
                        Navigation.RemovePage(Navigation.NavigationStack[currentPageIndex - 1]);
                    }
                }
                else
                {
                    await DisplayAlert("Update operation", $"User wasn't updated. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Update operation", $"User wasn't updated. {ex.Message}", "OK");
            }
        }
    }
}