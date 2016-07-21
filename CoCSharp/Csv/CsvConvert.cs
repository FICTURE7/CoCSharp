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
            s_defaultValueCache = new Hashtable();
            s_defaultValueCache.Add(typeof(int), default(int));
            s_defaultValueCache.Add(typeof(bool), default(bool));
        }

        private static readonly Hashtable s_defaultValueCache;
        private static readonly ObjectMapper s_mapper;

        /// <summary>
        /// Deserializes the specified <see cref="CsvTable"/> with the specified <typeparamref name="TCsvData"/>.
        /// </summary>
        /// <typeparam name="TCsvData">Type with which the <see cref="CsvTable"/> will be deserialized.</typeparam>
        /// <param name="table"><see cref="CsvTable"/> from which the data will be deserialize.</param>
        /// <returns>Returns the deserialized object array as the specified type <typeparamref name="TCsvData"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="table"/> is null.</exception>
        public static TCsvData[] Deserialize<TCsvData>(CsvTable table) where TCsvData : CsvData, new()
        {
            if (table == null)
                throw new ArgumentNullException("table");

            var type = typeof(TCsvData);
            // Map of all properties of the specified Type T.
            var propertyMap = s_mapper.MapProperties(type);

            var rows = table.Rows;

            var parentObj = (TCsvData)null;
            // Hashtable of property name and parameters(property values).
            var parentCache = new Hashtable();
            var objList = new List<TCsvData>();

            //var getterCalls = 0;
            //var setterCalls = 0;
            var index = -1;

            for (int i = 0; i < rows.Count; i++)
            {
                // We create a new instance of the Type T.
                var childObj = new TCsvData();

                // Set Properties value loop.
                for (int j = 0; j < propertyMap.Length; j++)
                {
                    var property = propertyMap[j];

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

                    var mustSet = false;
                    if (returnType.IsValueType)
                    {
                        if (!parameters[0].Equals(defaultValue))
                            mustSet = true;
                    }
                    else
                    {
                        mustSet = parameters[0] != null;
                    }

                    // If the property value is not a default value then
                    // we set it.
                    if (mustSet)
                    {
                        //setterCalls++;
                        property.Setter.Invoke(childObj, parameters);
                    }

                    // Check if the property == "Name" and if the property's value is not null.
                    // If it meets the conditions then it is a parent.
                    var isParent = property.PropertyName == "Name" && value != DBNull.Value;
                    if (isParent)
                    {
                        // Increase the index/level.
                        index++;
                        // Child object is now the parent object.
                        parentObj = childObj;
                        // Reset cache when we hit a new parent.
                        parentCache.Clear();
                    }
                }
                childObj.Index = index;

                objList.Add(childObj);
            }

            //Console.WriteLine(getterCalls);
            //Console.WriteLine(setterCalls);

            return objList.ToArray();
        }

        /// <summary>
        /// Deserializes the specified <see cref="CsvTable"/> with the specified <typeparamref name="TCsvData"/>.
        /// </summary>
        /// <typeparam name="TCsvData">Type with which the <see cref="CsvTable"/> will be deserialized.</typeparam>
        /// <param name="table"><see cref="CsvTable"/> from which the data will be deserialize.</param>
        /// <returns>A <see cref="CsvDataCollection{TCsvData}"/> which contains the deserialized objects.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="table"/> is null.</exception>
        public static CsvDataCollection<TCsvData> DeserializeNew<TCsvData>(CsvTable table) where TCsvData : CsvData, new()
        {
            if (table == null)
                throw new ArgumentNullException("table");

            var type = typeof(TCsvData);
            // Map of all properties of the specified Type T.
            var propertyMap = s_mapper.MapProperties(type);

            var rows = table.Rows;

            var parentObj = (TCsvData)null;
            // Hashtable of property name and parameters(property values).
            var parentCache = new Hashtable();
            var collection = new CsvDataCollection<TCsvData>();
            var subCollection = (CsvDataSubCollection<TCsvData>)null;

            //var getterCalls = 0;
            //var setterCalls = 0;
            var index = -1;
            var level = -1;

            for (int i = 0; i < rows.Count; i++)
            {
                // We create a new instance of the Type T.
                var childObj = new TCsvData();
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
                    if (!table.Columns.Contains(propertyName))
                        continue;

                    // Value from CSV table.
                    var value = curRow[propertyName];
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
                        // Increase the index because its a new parent.
                        index++;
                        // Reset the level because its a new parent.
                        level = -1;
                        // Child object is now the parent object.
                        parentObj = childObj;
                        // Reset cache when we hit a new parent.
                        parentCache.Clear();

                        if (subCollection != null)
                            collection.Add(subCollection);

                        subCollection = new CsvDataSubCollection<TCsvData>(parentObj.ID + index, parentObj.TID);
                    }
                }

                childObj.Index = index;
                childObj.Level = ++level;
                subCollection.Add(childObj);
            }

            //Console.WriteLine(getterCalls);
            //Console.WriteLine(setterCalls);

            // Last subCollection does not get added
            // so we add it here.
            collection.Add(subCollection); 
            return collection;
        }
    }
}
