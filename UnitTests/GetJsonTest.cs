using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTests
{
    [TestClass]
    public class GetJsonTest
    {
        public HttpClientBase httpClientBase = new HttpClientBase();

        [TestMethod]
        public void TestMethod1()
        {
            //string customersJson = "[" +
            //    "{\"id\":11,\"name\":\"Emma Sandoval\",\"phone\":\"1-939-105-3869\",\"email\":\"ligula.tortor.dictum@vel.net\",\"orders\":[]}," +
            //    "{\"id\":12,\"name\":\"Myles Henson\",\"phone\":\"1-143-153-7903\",\"email\":\"amet.ultricies@Cumsociis.com\",\"orders\":[]}," +
            //    "{\"id\":13,\"name\":\"Darius Armstrong\",\"phone\":\"1-457-315-8258\",\"email\":\"et.risus.Quisque@porttitorvulputateposuere.co.uk\",\"orders\":[]}]";

            //List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(customersJson);

            List<Customer> customers = httpClientBase.GetListItems<Customer>().Result;
            var customersNames = customers.Select(customer => customer.Name).ToList();
        }
    }
}
