using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRM.Views.BiReportsView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BiReports : ContentPage
	{
		public BiReports ()
		{
			  InitializeComponent ();

        if (App.IsUserLoggedIn)
        {
            MessageStackLayout.IsVisible = false;
            ReportWebView.IsVisible = true;
        }
        else
        {
            MessageStackLayout.IsVisible = true;
            ReportWebView.IsVisible = false;
        }
    }
	}
}