using System;
using System.Reflection;

namespace CoCSharp.Csv
{
    internal class ObjectMapper
    {
        public ObjectMapper()
        {
            // Space
        }

        public PropertyMap[] MapProperties(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var propertyMaps = new PropertyMap[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];

                var map = new PropertyMap();
                map.Name = CsvAttributeHelper.GetPropertyAlias(property);
                map.Ignore = CsvAttributeHelper.IsIgnored(property);
                map.PropertyName = property.Name;
                map.Getter = property.GetGetMethod(true);
                map.Setter = property.GetSetMethod(true);
                map.DeclaringType = property.DeclaringType;

                propertyMaps[i] = map;
            }
            return propertyMaps;
        }
    }
}
