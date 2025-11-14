using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace MyToolkit.Base.Converters
{
    public class EnumeratorToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string enumString;
            try
            {
                enumString = Enum.GetName((value.GetType()), value);
                return enumString;
            }
            catch
            {
                return string.Empty;
                // Returns empty string , if a exception is thrown then the view doesnt load up 
            }
        }


        //   Convert back is generally not require, we can leave it saying DoNothing of Binding type.
        public object ConvertBack(object value, Type targetType,
          object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
