using CRM.Models;
using CrmWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using CrmWebApp.Data;
using static CrmWebApp.Data.CommonData;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CrmWebApp.Controllers
{
    public class HomeController : Controller
    {
        CommonData commonData = new CommonData();

        public IActionResult Index()
        {
            ViewData["LoggedInUser"] = HttpContext.Session.Get<User>("LoggedInUser");
            ViewData["Error"] = null;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string Login, string Password)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{commonData.WebAPIUrl}/api/{CRM.Models.User.PluralDbTableName}/$Login={Login}&$Password={Password}"),
                    Method = HttpMethod.Get,
                    Headers = { { "Accept", "application/json" } }
                };

                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent content = response.Content;
                    string json = await content.ReadAsStringAsync();
                    User loggedInUser = JsonConvert.DeserializeObject<User>(json);

                    HttpContext.Session.Set<User>("LoggedInUser", loggedInUser);

                    ViewData["LoggedInUser"] = loggedInUser;
                    ViewData["Error"] = null;

                    return View();
                }
                else
                {
                    HttpContext.Session.Set<User>("LoggedInUser", null);

                    ViewData["LoggedInUser"] = null;

                    //sign out clicked -> (html.Login == null & html.Password == null)
                    ViewData["Error"] = (Login == null && Password == null) ? null : "Incorrect username/password !"; //response.StatusCode;

                    return View();
                }
            }
            catch (Exception)
            {
                HttpContext.Session.Set<User>("LoggedInUser", null);

                ViewData["LoggedInUser"] = null;

                //sign out clicked -> (html.Login == null & html.Password == null)
                ViewData["Error"] = (Login == null && Password == null) ? null : "Incorrect username/password !"; //ex.Message;

                return View();
            }
        }

        public IActionResult Users()
        {
            ViewData["Title"] = "Users";

            if (HttpContext.Session.Get<User>("LoggedInUser") != null)
            {
                ViewData["Message"] = $"{ViewData["Title"]} list";
                ViewData["Data"] = GetData<User>().Result;
            }
            else
            {
                ViewData["Message"] = "You must to sign in before viewing this page";
                ViewData["Data"] = null;
            }
            
            ViewData["Columns"] = GetHtmlTableColumns<User>();

            return View();
        }

        public IActionResult Payments()
        {
            ViewData["Title"] = "Payments";

            if (HttpContext.Session.Get<User>("LoggedInUser") != null)
            {
                ViewData["Message"] = $"{ViewData["Title"]} list";
                ViewData["Data"] = GetData<Payment>().Result;
            }
            else
            {
                ViewData["Message"] = "You must to sign in before viewing this page";
                ViewData["Data"] = null;
            }

            ViewData["Columns"] = GetHtmlTableColumns<Payment>();

            return View();
        }

        public IActionResult Orders()
        {
            ViewData["Title"] = "Orders";

            if (HttpContext.Session.Get<User>("LoggedInUser") != null)
            {
                ViewData["Message"] = $"{ViewData["Title"]} list";
                ViewData["Data"] = GetData<Order>().Result;
            }
            else
            {
                ViewData["Message"] = "You must to sign in before viewing this page";
                ViewData["Data"] = null;
            }

            ViewData["Columns"] = GetHtmlTableColumns<Order>();

            return View();
        }

        public IActionResult Customers()
        {
            ViewData["Title"] = "Customers";

            if (HttpContext.Session.Get<User>("LoggedInUser") != null)
            {
                ViewData["Message"] = $"{ViewData["Title"]} list";
                ViewData["Data"] = GetData<Customer>().Result;
            }
            else
            {
                ViewData["Message"] = "You must to sign in before viewing this page";
                ViewData["Data"] = null;
            }

            ViewData["Columns"] = GetHtmlTableColumns<Customer>();

            return View();
        }

        public IActionResult Departments()
        {
            ViewData["Title"] = "Departments";

            if (HttpContext.Session.Get<User>("LoggedInUser") != null)
            {
                ViewData["Message"] = $"{ViewData["Title"]} list";
                ViewData["Data"] = GetData<Department>().Result;
            }
            else
            {
                ViewData["Message"] = "You must to sign in before viewing this page";
                ViewData["Data"] = null;
            }

            ViewData["Columns"] = GetHtmlTableColumns<Department>();

            return View();
        }

        public IActionResult Reports()
        {
            if (HttpContext.Session.Get<User>("LoggedInUser") != null)
            {
                ViewData["Message"] = "";
            }
            else
            {
                ViewData["Message"] = "You must to sign in before viewing this page";
            }

            return View();
        }

        public List<HtmlTableColumn> GetHtmlTableColumns<T>()
        {
            List<HtmlTableColumn> columns = new List<HtmlTableColumn>() { };

            var typeProperties = typeof(T).GetProperties();

            foreach (var property in typeProperties)
                columns.Add(new HtmlTableColumn()
                {
                    field = property.Name.ToLower(),
                    title = property.Name,
                    sortable = property.Name.ToLower(),
                    show = true
                });

            foreach(var name in commonData.ignoredFields)
                columns.Remove(columns.Find(c => c.title.Equals(name)));

            return columns;
        }

        public async Task<List<T>> GetData<T>()
        {
            List<T> data = new List<T>() { };

            try
            {
                var pluralDbTableName = typeof(T).GetProperty("PluralDbTableName").GetValue(null);

                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{commonData.WebAPIUrl}/api/{pluralDbTableName}"),
                    Method = HttpMethod.Get,
                    Headers = { { "Accept", "application/json" } }
                };

                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    HttpContent content = response.Content;
                    string json = await content.ReadAsStringAsync();

                    data = JsonConvert.DeserializeObject<List<T>>(json);
                }

                return data;
            }
            catch (Exception)
            {
                return data;
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}