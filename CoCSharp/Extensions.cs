using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace CoCSharp
{
    internal static class StringBuilderExtensions
    {
        // increase fancyness
        public const string IndentString = "    ";

        public static void Indent(this StringBuilder builder)
        {
            builder.Append(IndentString);
        }

        public static void Indent(this StringBuilder builder, int indentLevel)
        {
            for (int i = 0; i < indentLevel; i++)
                builder.Append(IndentString);
        }

        public static void Indent(this StringBuilder builder, object value)
        {
            builder.Append(IndentString + value.ToString());
        }

        public static void Indent(this StringBuilder builder, object value, int indentLevel)
        {
            Indent(builder, indentLevel);
            builder.Append(value.ToString());
        }
    }


    internal static class ListExtension
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            //converts a List of <T> to DataTable for easier packet implementation
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }

}
