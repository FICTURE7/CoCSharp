using System;
using System.Collections.Generic;
using System.Reflection;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Provides methods to serialize and deserialize CsvTables to object array.
    /// Mainly designed for Clash of Clans.
    /// </summary>
    public static class CsvConvert
    {
        static CsvConvert()
        {
            s_csvDataType = typeof(CsvData);
            s_dataIndexProperty = s_csvDataType.GetProperty("DataIndex");
        }

        private static readonly Type s_csvDataType;
        private static readonly PropertyInfo s_dataIndexProperty;

        /// <summary>
        /// Deserializes the specified <see cref="CsvTable"/> with the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="table"><see cref="CsvTable"/> from which the data is deserialize.</param>
        /// <param name="type"><see cref="Type"/> of object to deserialize.</param>
        /// <returns>Returns the deserialized object array.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="table"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="type"/> is not a subclass of CoCData.</exception>
        public static object[] Deserialize(CsvTable table, Type type)
        {
            if (table == null)
                throw new ArgumentNullException("table");
            if (type == null)
                throw new ArgumentNullException("type");
            if (!type.IsSubclassOf(s_csvDataType))
                throw new ArgumentException("type is not a subclass of CsvData.");

            var rows = table.Rows;
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var parentObj = (object)null;
            var objList = new List<object>();
            var index = -1;

            for (int i = 0; i < rows.Count; i++)
            {
                var childObj = Activator.CreateInstance(type);

                for (int j = 0; j < properties.Length; j++) // set property value loop
                {
                    var property = properties[j];

                    if (property.Name == s_dataIndexProperty.Name && property.DeclaringType == s_csvDataType) // check if DataIndex
                    {
                        property.SetMethod.Invoke(childObj, new object[] { index });
                        continue;
                    }

                    if (CsvAttributeHelper.IsIgnored(property)) // ignore CsvIgnoreAttribute is there
                        continue;

                    var propertyName = CsvAttributeHelper.GetPropertyAlias(property);
                    if (!table.Columns.Contains(propertyName))
                        continue; // ignore if does not contian column

                    var value = rows[i][propertyName];
                    var parameters = (object[])null;

                    if (parentObj != null && value == DBNull.Value) // get data from parent
                        parameters = new object[] { property.GetMethod.Invoke(parentObj, null) };
                    else if (value == DBNull.Value)
                        continue; // keep default value
                    else
                        parameters = new object[] { value };

                    var isParent = property.Name == "Name" && value != DBNull.Value;
                    property.SetMethod.Invoke(childObj, parameters);

                    if (isParent)
                    {
                        index++;
                        parentObj = childObj;
                    }
                }
                objList.Add(childObj);
            }
            return objList.ToArray();
        }

        /// <summary>
        /// Deserializes the specified <see cref="CsvTable"/> with the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">Type with which the <see cref="CsvTable"/> will be deserialized.</typeparam>
        /// <param name="table"><see cref="CsvTable"/> from which the data will be deserialize.</param>
        /// <returns>Returns the deserialized object array as the specified type <c>T</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="table"/> is null.</exception>
        /// <exception cref="ArgumentException"><c>T</c> is not a subclass of CoCData.</exception>
        public static T[] Deserialize<T>(CsvTable table) where T : CsvData
        {
            var tType = typeof(T);
            var objs = Deserialize(table, tType);
            return Array.ConvertAll(objs, obj => (T)obj);
        }
    }
}
