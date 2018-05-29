using CRM.Data;
using CRM.Models;
using CRM.ViewModels;
using CRM.Views.UserView;
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

namespace CRM.Views.LoginView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            if (App.LoggedInUser == null)
            {
                UserPhoto.Source = Device.RuntimePlatform == Device.UWP ? "Assets/user_login.png" : "user_login.png";
                UserPhoto.HeightRequest = Device.RuntimePlatform == Device.UWP ? 200 : 110;
                ShowLoginPage();
            }
            else
            {
                ShowDashboard();
            }
        }

        public async void OnSignInButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}/$Login={LoginEntry.Text}&$Password={PasswordEntry.Text}"),
                    Method = HttpMethod.Get,
                    Headers = { { "Accept", "application/json" } }
                };

                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent content = response.Content;
                    string json = await content.ReadAsStringAsync();

                    App.LoggedInUser = JsonConvert.DeserializeObject<User>(json);
                    ShowDashboard();
                }
                else
                {
                    ShowLoginPage($"Incorrect username or password. {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                ShowLoginPage($"Incorrect username or password. {ex.Message}");
            }
        }

        protected void ShowDashboard()
        {
            LoggedInUserToolbarItem.Text = App.LoggedInUser.FullName;
            MessageStackLayout.IsVisible = false;
            SignInStackLayout.IsVisible = false;
            WelcomeLabel.Text += App.LoggedInUser.FullName + " !";
            WelcomeStackLayout.IsVisible = true;
        }

        protected void ShowLoginPage(string msg = null)
        {
            PasswordEntry.Text = string.Empty;

            if (msg != null)
            {
                MessageLabel.Text = msg;
                MessageStackLayout.IsVisible = true;
            }
        }

        public async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewUserPage());
        }
    }
}