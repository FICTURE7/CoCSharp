using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <param name="table"><see cref="CsvTable"/> from which the data</param>
        /// <param name="objectType"></param>
        /// <returns>Returns the deserializes objects.</returns>
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
                    var ignoreAttribute = property.GetCustomAttributes(typeof(CsvIgnoreAttribute), false);
                    if (ignoreAttribute.Length > 0)
                        continue; // 0 f***s given to properties with CsvIgnoreAttribute lal
                    var propertyAttribute = (CsvPropertyAttribute)property.GetCustomAttributes(typeof(CsvPropertyAttribute), false)
                                                                          .FirstOrDefault();
                    var propertyName = propertyAttribute == null ? property.Name : propertyAttribute.PropertyName;
                    var value = rows[i][propertyName];
                    var parameters = new object[] { value };
                    if (value == DBNull.Value) // still processing parentObj
                        continue;
                    else // use parentObj's value instead
                    {
                        parameters = new object[] { property.GetMethod.Invoke(parentObj, null) };
                        property.SetMethod.Invoke(childObj, parameters);
                    }
                    property.SetMethod.Invoke(childObj, parameters);
                }
                objList.Add(childObj);
            }
            return objList.ToArray();
        }
    }
}
