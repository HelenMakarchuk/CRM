using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public static class Constants
    {
        //appsettings.json can't be used when deploying on Azure (or I didn't find workaround), so I decided to use Constants.cs
        public static String DBConnectionString {
            get {
                return "Data Source=crmunivproject.database.windows.net;Initial Catalog=CRM_DB;Persist Security Info=True;User ID=helenamakarchuk;Password=gdhfb68t97BdfH";
            }
        }
    }
}
