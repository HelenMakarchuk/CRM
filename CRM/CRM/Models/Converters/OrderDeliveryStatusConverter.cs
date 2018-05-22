using System;
using System.Globalization;
using Xamarin.Forms;
using CRM.Data;
using System.ComponentModel;

namespace CRM.Models.Converters
{
    public class OrderDeliveryStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            try
            {
                //get enum value by index
                var item = ((OrderPickerData.DeliveryStatus)((byte)value));

                var field = item.GetType().GetField(item.ToString());
                var attr = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                var result = attr.Length == 0 ? item.ToString() : (attr[0] as DescriptionAttribute).Description;

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public object ConvertBack(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            try
            {
                var type = typeof(OrderPickerData.DeliveryStatus);
                OrderPickerData.DeliveryStatus status;

                foreach (var field in type.GetFields())
                {
                    var attribute = Attribute.GetCustomAttribute(field,
                        typeof(DescriptionAttribute)) as DescriptionAttribute;

                    if (attribute != null)
                    {
                        if (attribute.Description == (string)value)
                        {
                            status = (OrderPickerData.DeliveryStatus)field.GetValue(null);

                            //get enum index by value
                            return (byte)status;
                        }
                    }
                    else
                    {
                        if (field.Name == (string)value)
                        {
                            status = (OrderPickerData.DeliveryStatus)field.GetValue(null);

                            //get enum index by value
                            return (byte)status;
                        }
                    }
                }

                return "not found";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }    
    }
}
