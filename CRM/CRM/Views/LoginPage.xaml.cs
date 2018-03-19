using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //async void OnSignUpButtonClicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new SignUpPage());
        //}

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var user = new User
            {
                Login = loginEntry.Text,
                Password = passwordEntry.Text
            };

            var isValid = AreCredentialsCorrect(user);
            if (isValid)
            {
                App.IsUserLoggedIn = true;
                await Navigation.PushAsync(new Customers()); //MenuPage

                //Navigation.InsertPageBefore(new MenuPage(), this);
                //await Navigation.PopAsync();
            }
            else
            {
                //messageLabel.Text = "Login failed";
                //passwordEntry.Text = string.Empty;
            }
        }

        bool AreCredentialsCorrect(User user)
        {
            //search with linq users.where Login eq user.Login and Password eq user.Password

            //return user.Username == Constants.Username && user.Password == Constants.Password;

            return true;
        }
    }
}