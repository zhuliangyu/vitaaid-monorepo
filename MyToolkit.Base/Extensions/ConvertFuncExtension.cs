// reference: https://codingsight.com/simplifying-converters-for-wpf/

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

namespace MyToolkit.Base.Extensions
{
    public class ConvertFuncExtension : MarkupExtension
    {
        public ConvertFuncExtension()
        {
        }

        public ConvertFuncExtension(string functionsExpression)
        {
            FunctionsExpression = functionsExpression;
        }

        public string FunctionsExpression { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            object rootObject = (serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider).RootObject;
            MethodInfo convertMethod = null;
            MethodInfo convertBackMethod = null;

            ParseFunctionsExpression(out var convertType, out var convertMethodName, out var convertBackType, out var convertBackMethodName);

            if (convertMethodName != null)
            {
                var type = convertType ?? rootObject.GetType();
                var flags = convertType != null ?
                  BindingFlags.Public | BindingFlags.Static :
                  BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

                if ((convertMethod = type.GetMethod(convertMethodName, flags)) == null)
                    throw new ArgumentException($"Specified convert method {convertMethodName} not found on type {type.FullName}");
            }

            if (convertBackMethodName != null)
            {
                var type = convertBackType ?? rootObject.GetType();
                var flags = convertBackType != null ?
                  BindingFlags.Public | BindingFlags.Static :
                  BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

                if ((convertBackMethod = type.GetMethod(convertBackMethodName, flags)) == null)
                    throw new ArgumentException($"Specified convert method {convertBackMethodName} not found on type {type.FullName}");
            }

            return new Converter(rootObject, convertMethod, convertBackMethod);
        }

        void ParseFunctionsExpression(out Type convertType, out string convertMethodName, out Type convertBackType, out string convertBackMethodName)
        {
            if (!ParseFunctionsExpressionWithRegex(out string commonConvertTypeName, out string fullConvertMethodName, out string fullConvertBackMethodName))
                throw new ArgumentException("Error parsing functions expression");

            Lazy<Type[]> allTypes = new Lazy<Type[]>(GetAllTypes);

            Type commonConvertType = null;
            if (commonConvertTypeName != null)
            {
                commonConvertType = FindType(allTypes.Value, commonConvertTypeName);

                if (commonConvertType == null)
                    throw new ArgumentException($"Error parsing functions expression: type {commonConvertTypeName} not found");
            }

            convertType = commonConvertType;
            convertBackType = commonConvertType;

            if (fullConvertMethodName != null)
                ParseFullMethodName(allTypes, fullConvertMethodName, ref convertType, out convertMethodName);
            else
            {
                convertMethodName = null;
                convertBackMethodName = null;
            }

            if (fullConvertBackMethodName != null)
                ParseFullMethodName(allTypes, fullConvertBackMethodName, ref convertBackType, out convertBackMethodName);
            else
                convertBackMethodName = null;
        }

        bool ParseFunctionsExpressionWithRegex(out string commonConvertTypeName, out string fullConvertMethodName, out string fullConvertBackMethodName)
        {
            if (FunctionsExpression == null)
            {
                commonConvertTypeName = null;
                fullConvertMethodName = null;
                fullConvertBackMethodName = null;
                return true;
            }

            var match = _functionsExpressionRegex.Match(FunctionsExpression.Trim());

            if (!match.Success)
            {
                commonConvertTypeName = null;
                fullConvertMethodName = null;
                fullConvertBackMethodName = null;
                return false;
            }

            commonConvertTypeName = match.Groups[1].Value;
            if (commonConvertTypeName == "")
                commonConvertTypeName = null;

            fullConvertMethodName = match.Groups[2].Value.Trim();
            if (fullConvertMethodName == "")
                fullConvertMethodName = null;

            fullConvertBackMethodName = match.Groups[3].Value.Trim();
            if (fullConvertBackMethodName == "")
                fullConvertBackMethodName = null;

            return true;
        }

        static void ParseFullMethodName(Lazy<Type[]> allTypes, string fullMethodName, ref Type type, out string methodName)
        {
            var delimiterPos = fullMethodName.LastIndexOf('.');

            if (delimiterPos == -1)
            {
                methodName = fullMethodName;
                return;
            }

            methodName = fullMethodName.Substring(delimiterPos + 1, fullMethodName.Length - (delimiterPos + 1));

            var typeName = fullMethodName.Substring(0, delimiterPos);
            var foundType = FindType(allTypes.Value, typeName);
            type = foundType ?? throw new ArgumentException($"Error parsing functions expression: type {typeName} not found");
        }

        static Type FindType(Type[] types, string fullName)
          => types.FirstOrDefault(t => t.FullName.Equals(fullName));

        static Type[] GetAllTypes()
          => AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).ToArray();

        readonly Regex _functionsExpressionRegex = new Regex(
          @"^(?:([^ ,]+) )?([^,]+)(?:,([^,]+))?(?:[\s\S]*)$",
          RegexOptions.Compiled | RegexOptions.CultureInvariant);

        class Converter : IValueConverter, IMultiValueConverter
        {
            public Converter(object rootObject, MethodInfo convertMethod, MethodInfo convertBackMethod)
            {
                _rootObject = rootObject;
                _convertMethod = convertMethod;
                _convertBackMethod = convertBackMethod;

                _convertMethodParametersCount = _convertMethod != null ? _convertMethod.GetParameters().Length : 0;
                _convertBackMethodParametersCount = _convertBackMethod != null ? _convertBackMethod.GetParameters().Length : 0;
            }

            #region IValueConverter

            object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (_convertMethod == null)
                    return value;

                if (_convertMethodParametersCount == 1)
                    return _convertMethod.Invoke(_rootObject, new[] { value });
                else if (_convertMethodParametersCount == 2)
                    return _convertMethod.Invoke(_rootObject, new[] { value, parameter });
                else
                    throw new InvalidOperationException("Method has invalid parameters");
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (_convertBackMethod == null)
                    return value;

                if (_convertBackMethodParametersCount == 1)
                    return _convertBackMethod.Invoke(_rootObject, new[] { value });
                else if (_convertBackMethodParametersCount == 2)
                    return _convertBackMethod.Invoke(_rootObject, new[] { value, parameter });
                else
                    throw new InvalidOperationException("Method has invalid parameters");
            }

            #endregion

            #region IMultiValueConverter

            object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                if (_convertMethod == null)
                    throw new ArgumentException("Convert function is not defined");

                if (_convertMethodParametersCount == values.Length)
                    return _convertMethod.Invoke(_rootObject, values);
                else if (_convertMethodParametersCount == values.Length + 1)
                    return _convertMethod.Invoke(_rootObject, ConcatParameters(values, parameter));
                else
                    throw new InvalidOperationException("Method has invalid parameters");
            }

            object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                if (_convertBackMethod == null)
                    throw new ArgumentException("ConvertBack function is not defined");

                object converted;
                if (_convertBackMethodParametersCount == 1)
                    converted = _convertBackMethod.Invoke(_rootObject, new[] { value });
                else if (_convertBackMethodParametersCount == 2)
                    converted = _convertBackMethod.Invoke(_rootObject, new[] { value, parameter });
                else
                    throw new InvalidOperationException("Method has invalid parameters");

                if (converted is object[] convertedAsArray)
                    return convertedAsArray;

                // ToDo: Convert to object[] from Tuple<> and System.ValueTuple

                return null;
            }

            static object[] ConcatParameters(object[] parameters, object converterParameter)
            {
                object[] result = new object[parameters.Length + 1];
                parameters.CopyTo(result, 0);
                result[parameters.Length] = converterParameter;

                return result;
            }

            #endregion

            object _rootObject;

            MethodInfo _convertMethod;
            MethodInfo _convertBackMethod;

            int _convertMethodParametersCount;
            int _convertBackMethodParametersCount;
        }
    }
}
