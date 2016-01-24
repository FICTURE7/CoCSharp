using System;
using System.Reflection;

namespace CoCSharp.Csv
{
    internal static class CsvAttributeHelper
    {
        static CsvAttributeHelper()
        {
            s_csvAliasType = typeof(CsvAliasAttribute);
            s_csvIgnoreType = typeof(CsvIgnoreAttribute);
        }

        private static readonly Type s_csvAliasType;
        private static readonly Type s_csvIgnoreType;

        public static string GetPropertyAlias(PropertyInfo property)
        {
            var attributes = (CsvAliasAttribute[])property.GetCustomAttributes(s_csvAliasType, false);
            if (attributes.Length == 0)
                return property.Name;
            return attributes[0].Alias;
        }

        public static bool IsIgnored(PropertyInfo property)
        {
            return property.GetCustomAttributes(s_csvIgnoreType, false).Length > 0;
        }
    }
}
