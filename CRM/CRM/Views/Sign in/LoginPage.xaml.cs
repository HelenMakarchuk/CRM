using CRM.Models;
using CRM.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        User currentUser;

        public LoginPage()
        {
            InitializeComponent();

            UserPhoto.Source = Device.RuntimePlatform == Device.UWP ? "Assets/user_login.png" : "user_login.png";
            UserPhoto.HeightRequest = Device.RuntimePlatform == Device.UWP ? 200 : 110;

            this.Title = "Sign in";
            DashboardStackLayout.IsVisible = false;
            SignInStackLayout.IsVisible = true;
        }

        public async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}/{LoginEntry.Text}/{PasswordEntry.Text}"),
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
                    currentUser = JsonConvert.DeserializeObject<User>(json);

                    App.IsUserLoggedIn = true;
                    LoggedInUserToolbarItem.Text = currentUser.FullName;

                    MessageLabel.Text = "";
                    MessageStackLayout.IsVisible = false;

                    this.Title = "";
                    SignInStackLayout.IsVisible = false;
                    DashboardStackLayout.IsVisible = true;
                }
                catch (Exception ex)
                {
                    App.IsUserLoggedIn = false;
                    MessageLabel.Text = $"Login failed. {ex.Message}";
                    MessageStackLayout.IsVisible = true;
                    PasswordEntry.Text = string.Empty;
                }
            }
            else
            {
                App.IsUserLoggedIn = false;
                MessageLabel.Text = $"Login failed. Response status: {response.StatusCode}";
                MessageStackLayout.IsVisible = true;
                PasswordEntry.Text = string.Empty;
            }
        }
    }
}