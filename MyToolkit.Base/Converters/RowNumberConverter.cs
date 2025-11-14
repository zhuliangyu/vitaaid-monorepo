using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace MyToolkit.Base.Converters
{
    public class RowNumberConverter: IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            //get the grid and the item
            Object item = values[0];
            DataGrid grid = values[1] as DataGrid;
            if (grid == null) return "";
            int index = grid.Items.IndexOf(item) + 1;

            return index.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
