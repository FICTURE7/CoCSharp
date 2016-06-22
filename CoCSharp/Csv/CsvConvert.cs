using System;
using System.Collections;
using System.Collections.Generic;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Provides methods to serialize CsvTables to object array.
    /// Mainly designed for Clash of Clans.
    /// </summary>
    public static class CsvConvert
    {
        static CsvConvert()
        {
            s_mapper = new ObjectMapper();
            s_csvDataType = typeof(CsvData);
        }

        private static readonly ObjectMapper s_mapper;
        private static readonly Type s_csvDataType;

        /// <summary>
        /// Deserializes the specified <see cref="CsvTable"/> with the specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">Type with which the <see cref="CsvTable"/> will be deserialized.</typeparam>
        /// <param name="table"><see cref="CsvTable"/> from which the data will be deserialize.</param>
        /// <returns>Returns the deserialized object array as the specified type <c>T</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="table"/> is null.</exception>
        public static T[] Deserialize<T>(CsvTable table) where T : CsvData
        {
            if (table == null)
                throw new ArgumentNullException("table");

            var type = typeof(T);
            // Map of all properties of the specified Type T.
            var propertyMap = s_mapper.MapProperties(type);

            var rows = table.Rows;

            var parentObj = (T)null;
            var parentCache = new Hashtable();
            var objList = new List<T>();

            //var getterCalls = 0;
            var index = -1;

            for (int i = 0; i < rows.Count; i++)
            {
                // We create a new instance of the Type type.
                var childObj = (T)Activator.CreateInstance(type);

                // Set Properties value loop.
                for (int j = 0; j < propertyMap.Length; j++)
                {
                    var property = propertyMap[j];

                    // Ignore the property if it has a CsvIgnoreAttribute attached to it.
                    if (property.Ignore)
                        continue;

                    // Gets the name of the property and taking in consideration CsvAliasAttribute.
                    var propertyName = property.Name;

                    // Check if the table contains a column with its name == propertyName.
                    // If the table does not then we ignore the property.
                    if (!table.Columns.Contains(propertyName))
                        continue;

                    // Value from CSV table.
                    var value = rows[i][propertyName];
                    // Parameters we will pass to the property.
                    var parameters = (object[])null;

                    // Get value from parent property if we don't have 
                    // the value from table(empty field).
                    if (value == DBNull.Value)
                    {
                        if (parentObj != null)
                        {
                            // Could cache this thing.

                            // Look up property value in cache.
                            // If we don't have it cached we cache it.
                            if (!parentCache.Contains(property.Getter.Name))
                            {
                                //getterCalls++;
                                parameters = new object[]
                                {
                                    property.Getter.Invoke(parentObj, null)
                                };
                                parentCache.Add(property.Getter.Name, parameters);
                            }
                            else
                            {
                                parameters = (object[])parentCache[property.Getter.Name];
                            }
                        }
                    }
                    else
                    {
                        // Use value from table if its available in the table.
                        parameters = new object[]
                        {
                            value
                        };

                        // If the property value has changed,
                        // we update the cache.
                        if (parentCache.Contains(property.Getter.Name))
                        {
                            parentCache[property.Getter.Name] = parameters;
                        }
                        else
                        {
                            parentCache.Add(property.Getter.Name, parameters);
                        }
                    }

                    // Check if the property == "Name" and if the property's value is not null.
                    // If it meets the conditions then it is a parent.
                    var isParent = property.PropertyName == "Name" && value != DBNull.Value;
                    property.Setter.Invoke(childObj, parameters);

                    if (isParent)
                    {
                        index++;
                        parentObj = childObj;

                        // Reset cache when we have a new parent.
                        parentCache = new Hashtable();
                    }
                }
                childObj.Index = index;

                objList.Add(childObj);
            }

            //Console.WriteLine(getterCalls);

            return objList.ToArray();
        }
    }
}
