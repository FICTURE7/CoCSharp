using System;
using System.Collections;

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
            s_defaultValueCache = new Hashtable();
            s_defaultValueCache.Add(typeof(int), default(int));
            s_defaultValueCache.Add(typeof(bool), default(bool));
        }

        private static readonly Hashtable s_defaultValueCache;
        private static readonly ObjectMapper s_mapper;

        /// <summary>
        /// Deserializes the specified <see cref="CsvTable"/> into a <see cref="CsvDataTable{TCsvData}"/> with type as generic
        /// argument.
        /// </summary>
        /// <param name="table"><see cref="CsvTable"/> to deserialize.</param>
        /// <param name="type">Type of <see cref="CsvData"/> to deserialize.</param>
        /// <returns>
        /// Deserialized <see cref="CsvDataTable{TCsvData}"/> with <paramref name="type"/> as generic argument.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="table"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="type"/> does not inherit from <see cref="CsvData"/> or is an abstract class.</exception>
        public static CsvDataTable Deserialize(CsvTable table, Type type)
        {
            if (table == null)
                throw new ArgumentNullException("table");
            if (type == null)
                throw new ArgumentNullException("type");
            if (type.IsAbstract || type.BaseType != typeof(CsvData))
                throw new ArgumentException("type must be inherited from CsvData type and non-abstract.");

            // Instance of the CsvDataTable<type> we're going to return.
            var dataRow = CsvDataTable.CreateInstance(type);
            // Instance of the CsvDataRow<type> we're going to add to dataTable at the beginning
            // of a new parent.
            var dataCollection = (CsvDataRow)null;
            // Map of all properties of the specified Type T.
            var propertyMap = s_mapper.MapProperties(type, table);

            var rows = table.Rows;

            var parentObj = (object)null;
            // Hashtable of property name and parameters(property values).
            var parentCache = new Hashtable();

            //var getterCalls = 0;
            //var setterCalls = 0;

            for (int i = 0; i < rows.Count; i++)
            {
                // We create a new instance of the Type T.
                var childObj = (CsvData)Activator.CreateInstance(type);
                var curRow = rows[i];
                var isParent = false;

                // Set Properties value loop.
                for (int j = 0; j < propertyMap.Length; j++)
                {
                    var property = propertyMap[j];

                    // Gets the name of the property and taking in consideration CsvAliasAttribute.
                    var propertyName = property.Name;

                    // Check if the table contains a column with its name == propertyName.
                    // If the table does not then we ignore the property.
                    //if (!table.Columns.Contains(propertyName))
                    //    continue;

                    // Value from CSV table.
                    var value = curRow[property.ColumnIndex];
                    // Parameters we will pass to the property.
                    var parameters = (object[])null;

                    // Get value from parent property if we don't have 
                    // the value from table(empty field).
                    if (value == DBNull.Value)
                    {
                        if (parentObj != null)
                        {
                            // Look up property value in cache.
                            // If we don't have it cached we cache it.
                            if (!parentCache.Contains(property.PropertyName))
                            {
                                //getterCalls++;
                                parameters = new object[]
                                {
                                    property.Getter.Invoke(parentObj, null)
                                };
                                parentCache.Add(property.PropertyName, parameters);
                            }
                            else
                            {
                                parameters = (object[])parentCache[property.PropertyName];
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

                        // We update the cache or add it to the cache if it isn't cached.
                        if (parentCache.Contains(property.PropertyName))
                        {
                            parentCache[property.PropertyName] = parameters;
                        }
                        else
                        {
                            parentCache.Add(property.PropertyName, parameters);
                        }
                    }

                    // Set the property value on the child object.
                    // Cache default value of Property Type.
                    var returnType = property.PropertyType;
                    var defaultValue = (object)null;

                    if (returnType.IsValueType)
                    {
                        // Look up for default value in cache.
                        // Add default value to cache if its not cached.
                        if (!s_defaultValueCache.Contains(returnType))
                        {
                            defaultValue = Activator.CreateInstance(property.Getter.ReturnType);
                            s_defaultValueCache.Add(returnType, defaultValue);
                        }
                        else
                        {
                            defaultValue = s_defaultValueCache[returnType];
                        }
                    }

                    var set = false;
                    if (returnType.IsValueType)
                    {
                        if (!parameters[0].Equals(defaultValue))
                            set = true;
                    }
                    else
                    {
                        set = parameters[0] != null;
                    }

                    // If the property value is not a default value then
                    // we set it.
                    if (set)
                    {
                        //setterCalls++;
                        property.Setter.Invoke(childObj, parameters);
                    }

                    // Check if the property' name == "Name" and if the property's value is not null.
                    // If it meets the conditions then it is a parent.
                    // Because parents and children share the same name.
                    isParent = property.PropertyName == "Name" && value != DBNull.Value;
                    if (isParent)
                    {
                        dataCollection = CsvDataRow.CreateInstance(type, (string)parameters[0]);
                        dataRow.ProxyAdd(dataCollection);

                        // Child object is now the parent object.
                        parentObj = childObj;
                        // Reset cache when we hit a new parent.
                        parentCache.Clear();
                    }
                }
                dataCollection.ProxyAdd(childObj);
            }

            //Console.WriteLine(getterCalls);
            //Console.WriteLine(setterCalls);
            return dataRow;
        }
    }
}
