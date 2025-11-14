using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyToolkit.Base.Converters
{
	public class GenericConverter : IValueConverter, IMultiValueConverter
	{
		private Func<object, Type, object, CultureInfo, object> _Convert { get; set; } = null;
		private Func<object, Type, object, CultureInfo, object> _ConvertBack { get; set; } = null;
		private Func<object[], Type, object, CultureInfo, object> _MultiConvert { get; set; } = null;
		private Func<object, Type[], object, CultureInfo, object[]> _MultiConvertBack { get; set; } = null;
		public GenericConverter(
			Func<object, Type, object, CultureInfo, object> convert = null,
			Func<object, Type, object, CultureInfo, object> convertBack = null,
			Func<object[], Type, object, CultureInfo, object> multiConvert = null,
			Func<object, Type[], object, CultureInfo, object[]> multiConvertBack = null
			)

		{
			// Store conversion methods 
			_Convert = convert;
			_ConvertBack = convertBack;
			_MultiConvert = multiConvert;
			_MultiConvertBack = multiConvertBack;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// Call conversion by a delegate converting input parameters 
			return _Convert?.Invoke(value, targetType, parameter, culture) ?? null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// Call conversion by a delegate converting input parameters
			return _ConvertBack?.Invoke(value, targetType, parameter, culture) ?? null;
		}

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			// Call conversion by a delegate converting input parameters
			return _MultiConvert?.Invoke(values, targetType, parameter, culture) ?? null;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			// Call conversion by a delegate converting input parameters
			return _MultiConvertBack?.Invoke(value, targetTypes, parameter, culture) ?? null;
		}
	}
}