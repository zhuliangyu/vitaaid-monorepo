using System;

namespace MySystem.Base.Extensions
{
    public static class ObjectExtension
    {
        // Kotlin: fun <T, R> T.let(block: (T) -> R): R
        public static R Let<T, R>(this T self, Func<T, R> block)
        {
            return block(self);
        }

        //public static void Let<T>(this T self, Action<T> block)
        //{
        //    block(self);
        //}

        // Kotlin: fun <T> T.also(block: (T) -> Unit): T
        public static T Also<T>(this T self, Action<T> block)
        {
            block(self);
            return self;
        }

        public static int ToInt(this object self)
        {
            return (self is int) ? (int)self :
                   (self is decimal || self is decimal?) ? (int)(decimal)self :
                   (self is double || self is double?) ? (int)(double)self :
                   (self is float || self is float?) ? (int)(float)self :
                   (self is short || self is short?) ? (int)(short)self :
                   (self is bool || self is bool?) ? ((bool)self) ? 1 : 0 :
                   (self is string) ? StringToInt((string) self) :
                   throw new Exception("Can not convert object to int.");
        }

        public static double ToDouble(this object self)
        {
            return (self is int) ? (double)(int)self :
                   (self is decimal || self is decimal?) ? (double)(decimal)self :
                   (self is double || self is double?) ? (double)self :
                   (self is float || self is float?) ? (double)(float)self :
                   (self is short || self is short?) ? (double)(short)self :
                   (self is bool || self is bool?) ? ((bool)self) ? 1 : 0 :
                   (self is string) ? StringToDouble((string)self) :
                   throw new Exception("Can not convert object to int.");
        }

        public static bool ToBool(this object self)
        {
            return (self is int) ? ((int)self == 1) :
                   (self is double || self is double?) ? (((int)(double)self) == 1) :
                   (self is float || self is float?) ? (((int)(float)self) == 1) :
                   (self is short || self is short?) ? (((int)(short)self) == 1) :
                   (self is bool || self is bool?) ? (bool)self :
                   throw new Exception("Can not convert object to bool.");
        }

        public static int StringToInt(this string source, int Default = 0)
        {
            int iVal = Default;
            if (Int32.TryParse(source, out iVal) == true)
                return Int32.Parse(source);
            else
                return Default;
        }
        public static double StringToDouble(this string source, double Default = 0.0)
        {
            double dVal = Default;
            if (Double.TryParse(source, out dVal) == true)
                return dVal;
            else
                return Default;
        }


        public static bool LogicalEqual(this object source, object target)
        {
            if (source is DateTime)
                return DateTimeLogicalEqual(source, target);
            if (source is string)
                return StringLogicalEqual(source as String, target as String);
            if (source == null && target == null)
                return true;
            if (source == null || target == null)
                return false;
            return source.Equals(target);
        }

        private static bool DateTimeLogicalEqual(object source, object target)
        {
            if (source == null && ((DateTime)target) == DateTime.MinValue)
                return true;
            if (target == null && ((DateTime)source) == DateTime.MinValue)
                return true;
            return ((DateTime)source) == ((DateTime)target);
        }

        private static bool StringLogicalEqual(String source, String target)
        {
            if (string.IsNullOrWhiteSpace(source) && string.IsNullOrWhiteSpace(target))
                return true;
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(target))
                return false;
            return source.Equals(target);
        }

    }
}