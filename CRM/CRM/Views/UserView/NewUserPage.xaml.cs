using CRM.Data;
using CRM.Models;
using CRM.Views.LoginView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views.UserView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewUserPage : ContentPage
    {
        public NewUserPage()
        {
            InitializeComponent();

            SaveToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/save.png" : "save.png";

            GenderPicker.ItemsSource = UserPickerData.genders.Values.ToList();
            FillDepartmentPicker();

            BindingContext = this;
        }

        public NewUserPage(Department department)
        {
            InitializeComponent();

            SaveToolbarItem.Icon = Device.RuntimePlatform == Device.UWP ? "Assets/save.png" : "save.png";

            GenderPicker.ItemsSource = UserPickerData.genders.Values.ToList();
            FillDepartmentPicker(department);

            BindingContext = this;
        }

        protected async void FillDepartmentPicker(Department department = null)
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

                    if (department != null)
                        DepartmentPicker.SelectedItem = departments.Where(d => d.Id == department.Id).SingleOrDefault();
                }
                catch (Exception)
                {
                    DepartmentPicker.ItemsSource = new List<Department>();
                }
            }
        }

        async void CreateUser(User user)
        {
            try
            {
                string json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json);

                var client = new HttpClient();
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}", content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    App.LoggedInUser = JsonConvert.DeserializeObject<User>(json);
                    await DisplayAlert("Create operation", $"User \"{FullNameEntry.Text}\" was created.", "OK");

                    if (Navigation.NavigationStack.Count > 0)
                    {
                        int currentPageIndex = Navigation.NavigationStack.Count - 1;
                        Navigation.RemovePage(Navigation.NavigationStack[currentPageIndex - 1]);
                        await Navigation.PushAsync(new LoginPage());
                        Navigation.RemovePage(Navigation.NavigationStack[currentPageIndex - 1]);
                    }
                }
                else
                {
                    await DisplayAlert("Create operation", $"User \"{FullNameEntry.Text}\" wasn't created. Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Create operation", $"An error occured while creating user \"{FullNameEntry.Text}\". {ex.Message}", "OK");
            }
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                #region Checks

                if (LoginEntry.Text == string.Empty || LoginEntry.Text == null)
                {
                    await DisplayAlert("Create operation", "Login must be set", "OK");
                    return;
                }

                if (PasswordEntry.Text == string.Empty || PasswordEntry.Text == null)
                {
                    await DisplayAlert("Create operation", "Password must be set", "OK");
                    return;
                }

                #endregion

                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}/Login={LoginEntry.Text}/$exists"),
                    Method = HttpMethod.Get,
                    Headers = { { "Accept", "application/json" } }
                };

                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent content = response.Content;
                    string json = await content.ReadAsStringAsync();

                    bool loginExists = JsonConvert.DeserializeObject<bool>(json);

                    if (!loginExists)
                    {
                        #region New user assembling

                        User user = new User();

                        if (FullNameEntry.Text != string.Empty)
                            user.FullName = FullNameEntry.Text;

                        if (EmailEntry.Text != string.Empty)
                            user.Email = EmailEntry.Text;

                        if (PositionEntry.Text != string.Empty)
                            user.Position = PositionEntry.Text;

                        if (PhoneEntry.Text != string.Empty)
                            user.Phone = PhoneEntry.Text;

                        if (LoginEntry.Text != string.Empty)
                            user.Login = LoginEntry.Text;

                        if (PasswordEntry.Text != string.Empty)
                            user.Password = PasswordEntry.Text;

                        user.BirthDate = BirthDatePicker.Date;

                        var selectedGender = GenderPicker.SelectedItem?.ToString() ?? "";
                        if (UserPickerData.genders.ContainsValue(selectedGender))
                        {
                            user.Gender = UserPickerData.genders.FirstOrDefault(x => x.Value == selectedGender).Key;
                        }

                        //if (DepartmentPicker.SelectedIndex == 0) { user want to leave department empty (null) } 
                        if (DepartmentPicker.SelectedIndex != 0 && DepartmentPicker.SelectedItem is Department selectedDepartment)
                        {
                            user.DepartmentId = selectedDepartment.Id;
                        }

                        #endregion

                        CreateUser(user);
                    }
                    else
                    {
                        await DisplayAlert("Check operation", $"Another user already exists in the system with the same login.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Check operation", $"Response status: {response.StatusCode}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Check operation", $"An error occured while checking login \"{LoginEntry.Text}\". {ex.Message}", "OK");
            }
        }
    }
}