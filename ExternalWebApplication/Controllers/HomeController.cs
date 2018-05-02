using CRM.Models;
using ExternalWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExternalWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public class HtmlTableColumn
        {
            public string field;
            public string title;
            public string sortable;
            public bool show;
        }

        public string[] ignoredFields = new string[] {
            "PluralDbTableName",
            "Employees",
            "Head",
            "Payments",
            "Order",
            "HeadedDepartments",
            "WorkplaceDepartment"
        };

        public static string WebAPIUrl = "http://webapi20180417071917.azurewebsites.net";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            ViewData["Title"] = "Users";
            ViewData["Message"] = $"{ViewData["Title"]} list";
            ViewData["Data"] = GetData<User>().Result;
            ViewData["Columns"] = GetHtmlTableColumns<User>();

            return View();
        }

        public IActionResult Payments()
        {
            ViewData["Title"] = "Payments";
            ViewData["Message"] = $"{ViewData["Title"]} list";
            ViewData["Data"] = GetData<Payment>().Result;
            ViewData["Columns"] = GetHtmlTableColumns<Payment>();

            return View();
        }

        public IActionResult Orders()
        {
            ViewData["Title"] = "Orders";
            ViewData["Message"] = $"{ViewData["Title"]} list";
            ViewData["Data"] = GetData<Order>().Result;
            ViewData["Columns"] = GetHtmlTableColumns<Order>();

            return View();
        }

        public IActionResult Customers()
        {
            ViewData["Title"] = "Customers";
            ViewData["Message"] = $"{ViewData["Title"]} list";
            ViewData["Data"] = GetData<Customer>().Result;
            ViewData["Columns"] = GetHtmlTableColumns<Customer>();

            return View();
        }

        public IActionResult Departments()
        {
            ViewData["Title"] = "Departments";
            ViewData["Message"] = $"{ViewData["Title"]} list";
            ViewData["Data"] = GetData<Department>().Result;
            ViewData["Columns"] = GetHtmlTableColumns<Department>();

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

            foreach(var name in ignoredFields)
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
                    RequestUri = new Uri($"{WebAPIUrl}/api/{pluralDbTableName}"),
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
