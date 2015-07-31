using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoCSharp.Data.Csv
{
    /// <summary>
    /// Provides methods to serialize and deserialize CsvTables to object array.
    /// </summary>
    public class CsvSerializer
    {
        /// <summary>
        /// Deserializes the specified <see cref="CsvTable"/> with the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="table"><see cref="CsvTable"/> from which the data is deserialize.</param>
        /// <param name="objectType"><see cref="Type"/> of object to deserialize.</param>
        /// <returns>Returns the deserialized objects.</returns>
        public static object[] Deserialize(CsvTable table, Type objectType)
        {
            var rows = table.Rows;
            var properties = objectType.GetProperties();
            var parentObj = (object)null;
            var objList = new List<object>();

            for (int x = 0; x < rows.Count; x++)
            {
                var childObj = Activator.CreateInstance(objectType);
                for (int i = 0; i < properties.Length; i++) // set property value loop
                {
                    var property = properties[i];
                    if (HasIgnoreAttribute(property))
                        continue; // ignore CsvIgnoreAttribute

                    var propertyName = GetPropertyAttributeName(property);
                    var value = rows[x][propertyName];
                    var parameters = new object[] { value };

                    if (parentObj != null && value == DBNull.Value) // get data from parent
                        parameters = new object[] { property.GetMethod.Invoke(parentObj, null) };
                    else if (value == DBNull.Value)
                        continue;

                    var isParent = property.Name == "Name" && value != DBNull.Value;
                    property.SetMethod.Invoke(childObj, parameters);

                    if (isParent)
                        parentObj = childObj;
                }
                objList.Add(childObj);
            }
            return objList.ToArray();
        }

        private static string GetPropertyAttributeName(PropertyInfo property)
        {
            var propertyAttribute = (CsvPropertyAttribute)property.GetCustomAttributes(typeof(CsvPropertyAttribute), false)
                                                                          .FirstOrDefault();
            return propertyAttribute == null ? property.Name : propertyAttribute.PropertyName;
        }

        private static bool HasIgnoreAttribute(PropertyInfo property)
        {
            var ignoreAttribute = property.GetCustomAttributes(typeof(CsvIgnoreAttribute), false);
            if (ignoreAttribute.Length > 0)
                return true;
            return false;
        }
    }
}
