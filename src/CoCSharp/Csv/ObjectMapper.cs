using System;
using System.Collections.Generic;
using System.Reflection;

namespace CoCSharp.Csv
{
    internal class ObjectMapper
    {
        private const string NameColumn = "Name";

        public ObjectMapper()
        {
            // Space
        }

        public PropertyMap[] MapProperties(Type type, CsvTable table)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var propertyMaps = new List<PropertyMap>();
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                // If we need to ignore it, don't add the property to the map.
                if (CsvAttributeHelper.IsIgnored(property))
                    continue;
                // If it does not have a setter, we ignore it.
                var setter = property.GetSetMethod(true);
                if (setter == null)
                    continue;
                // If it does not have a getter, we ignore it.
                var getter = property.GetGetMethod(true);
                if (getter == null)
                    continue;

                var map = new PropertyMap();
                var name = CsvAttributeHelper.GetPropertyAlias(property);
                var index = table.Columns.IndexOf(name);
                if (index == -1)
                    continue;

                map.Name = name;
                map.ColumnIndex = index;
                map.PropertyName = property.Name;
                map.PropertyType = property.PropertyType;
                map.Getter = getter;
                map.Setter = setter;
                map.DeclaringType = property.DeclaringType;

                // Make sure TID property is first.
                if (map.Name == NameColumn)
                    propertyMaps.Insert(0, map);
                else
                    propertyMaps.Add(map);
            }
            return propertyMaps.ToArray();
        }
    }
}
