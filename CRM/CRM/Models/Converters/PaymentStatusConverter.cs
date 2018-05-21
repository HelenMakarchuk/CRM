using System;
using System.Globalization;
using Xamarin.Forms;
using CRM.Data;
using System.ComponentModel;
using System.Collections.Generic;

namespace CRM.Models.Converters
{
    public class PaymentStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            try
            {
                //get enum value by index
                var item = ((PaymentPickerData.Status)((byte)value));

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

        public List<string> ConvertAll()
        {
            try
            {
                var type = typeof(PaymentPickerData.Status);
                var descriptions = new List<string>();
                var values = Enum.GetValues(type);

                foreach (var value in values)
                {
                    var field = value.GetType().GetField(value.ToString());
                    var attr = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    descriptions.Add(attr.Length == 0 ? value.ToString() : (attr[0] as DescriptionAttribute).Description);
                }

                return descriptions;
            }
            catch (Exception ex)
            {
                return new List<string>() { ex.Message };
            }
        }

        public object ConvertBack(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            try
            {
                var type = typeof(PaymentPickerData.Status);
                PaymentPickerData.Status status;

                foreach (var field in type.GetFields())
                {
                    var attribute = Attribute.GetCustomAttribute(field,
                        typeof(DescriptionAttribute)) as DescriptionAttribute;

                    if (attribute != null)
                    {
                        if (attribute.Description == (string)value)
                        {
                            status = (PaymentPickerData.Status)field.GetValue(null);

                            //get enum index by value
                            return (int)status;
                        }
                    }
                    else
                    {
                        if (field.Name == (string)value)
                        {
                            status = (PaymentPickerData.Status)field.GetValue(null);

                            //get enum index by value
                            return (int)status;
                        }
                    }
                }

                return -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
