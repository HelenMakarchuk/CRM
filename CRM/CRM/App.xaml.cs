using CRM.Models;
using CRM.Views;
using CRM.ViewModels;
using Xamarin.Forms;
using CRM.Views.MenuView;

namespace CRM
{
    public partial class App : Application
	  {
        public App()
        {
            InitializeComponent();
            MainPage = new MenuPage();
        }

        public static User LoggedInUser { get; set; }

        protected override void OnStart ()
        {
            // Handle when your app starts
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
        }
	  }
}
