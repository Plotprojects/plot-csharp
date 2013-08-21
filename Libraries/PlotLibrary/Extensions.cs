using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;

namespace PlotLibrary.Extensions
{
    /// <summary>
    /// Helpers for date format (like ISO-8601)
    /// </summary>
    static class DateTimeExtensions
    {
        /// <summary>
        /// Convert a string value to a datetime value, optionally indicating if an empty string is allow (and will yield a 'null'). The string must be in ISO-8601 format
        /// </summary>
        /// <param name="s">String value in ISO-8601 format (or null)</param>
        /// <param name="mayNotBeEmpty">Indicates if an empty string is allowed; default false</param>
        /// <returns>Converted datetime (or 'null')</returns>
        static public DateTime? AsDateTime(this string s, Boolean mayNotBeEmpty = false) // mayNotBeEmpty can also be read as 'throwIfEmpty' or 'mustBeValidDate'
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                if (mayNotBeEmpty)
                    throw new Exception("May not be empty");
                return null;
            }
            DateTime d;
            if (!DateTime.TryParseExact(s, @"yyyy-MM-dd\THH:mm:ss\Z", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out d)) // with the Z
                if (!DateTime.TryParseExact(s, "s", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out d)) // works if no Z at the end
                    throw new Exception(string.Format("Invalid date '{0}'", s));
            return d;
        }

        /// <summary>
        /// Convert a datetime to a string value in ISO-8601 format
        /// </summary>
        /// <param name="dt">Datetime to be converted</param>
        /// <returns>Converted string (or 'null')</returns>
        static public string AsString(this DateTime? dt)
        {
            if (!dt.HasValue)
                return null;
            return dt.Value.ToUniversalTime().ToString("s") + "Z";
        }

        /// <summary>
        /// Convert a datetime to a string value in 'yyyy-mm-dd' format
        /// </summary>
        /// <param name="dt">Datetime to be converted</param>
        /// <returns>Converted string (or 'null')</returns>
        static public string AsStringYMD(this DateTime? dt)
        {
            if (!dt.HasValue)
                return null;
            return dt.Value.ToString("yyyy-MM-dd");
        }
    }

    /// <summary>
    /// (Type safe) helpers for enums (and strings)
    /// </summary>
    static class EnumExtensions
    {
        /// <summary>
        /// Generic methof to (type safe) convert a string value to it's enum value
        /// </summary>
        /// <typeparam name="TEnum">Type of the enum to return</typeparam>
        /// <param name="enumString">String value to be converted to an enum</param>
        /// <returns>(type safe) enum value of the converted string</returns>
        public static TEnum AsEnum<TEnum>(this string enumString) where TEnum : struct
        {
            if (string.IsNullOrWhiteSpace(enumString))
                throw new Exception(string.Format("Cannot convert empty value to enum '{0}'", typeof(TEnum).Name));
            TEnum ret;
            if (!Enum.TryParse(enumString, true, out ret))
                throw new Exception(string.Format("Cannot convert '{0}' to enum '{1}'", enumString, typeof(TEnum).Name));
            return ret;
        }
    }
}
