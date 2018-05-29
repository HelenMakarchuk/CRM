using System;
using CRM.Views.WebsiteView;

namespace CRM.Models
{
    /// <summary>
    /// Menu item
    /// </summary>
    public class Item
    {
        public Type TargetType { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageSource { get; set; }

        public Item()
        {
            TargetType = typeof(Website);
        }
    }
}