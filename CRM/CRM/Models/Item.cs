using System;

using CRM.Views;

namespace CRM.Models
{
    public class Item
    {
        public Type TargetType { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string ImageSource { get; set; }

        public Item()
        {
            TargetType = typeof(Website);
        }
    }
}