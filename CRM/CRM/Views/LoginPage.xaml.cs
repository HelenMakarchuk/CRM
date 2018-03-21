using CRM.Models;
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
		    public LoginPage ()
		    {
			      InitializeComponent ();
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuPage());
        }

        public async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{Constants.WebAPIUrl}/api/{User.PluralDbTableName}/{loginEntry.Text}/{passwordEntry.Text}"),
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
                    string Id = JsonConvert.DeserializeObject<string>(json);

                    if (Int32.TryParse(Id, out var i))
                    {
                        messageLabel.Text = "";
                        messageStackLayout.IsVisible = false;
                        App.IsUserLoggedIn = true;
                        Navigation.InsertPageBefore(new MenuPage(), this);
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        messageLabel.Text = "Login failed";
                        messageStackLayout.IsVisible = true;
                        passwordEntry.Text = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    messageLabel.Text = $"Login failed. {ex.Message}";
                    messageStackLayout.IsVisible = true;
                    passwordEntry.Text = string.Empty;
                }
            }
        }
    }
}