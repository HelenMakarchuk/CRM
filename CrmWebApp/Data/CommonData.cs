using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrmWebApp.Data
{
    public class CommonData
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

        public string WebAPIUrl { get { return "http://webapi20180531093609.azurewebsites.net"; } }
    }
}
