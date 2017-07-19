using Game.Model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Game.UI.Converter
{
    public class EmploymentStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? d = value as DateTime?;
            
            if (d == null)
                    return "employed";
            else
                return "leaves on " + d.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
