using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CargadosTrucking.Helpers
{
    public class VimHelper : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
                return ((string)value).Substring(((string)value).Length-4);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
                return !(bool)value;
            return value;
        }
    }
}
