using System;
using System.Collections.Generic;
using System.Reflection;

namespace CoCSharp.Csv
{
    internal class ObjectMapper
    {
        private const string TIDName = "TID";

        public ObjectMapper()
        {
            // Space
        }

        public PropertyMap[] MapProperties(Type type)
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
                map.Name = CsvAttributeHelper.GetPropertyAlias(property);
                map.PropertyName = property.Name;
                map.PropertyType = property.PropertyType;
                map.Getter = getter;
                map.Setter = setter;
                map.DeclaringType = property.DeclaringType;

                // Make sure TID property is first.
                if (map.Name == TIDName)
                    propertyMaps.Insert(0, map);
                else
                    propertyMaps.Add(map);
            }
            return propertyMaps.ToArray();
        }
    }
}
