using System;
using System.Globalization;
using System.Windows.Data;

namespace Game.UI.Converter
{
    public class ProjectStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool status = (bool)value;
            
            if (status)
                    return "ongoing";
            else
                return "stopped";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
