using System;
using System.Reflection;

namespace CoCSharp.Csv
{
    internal class PropertyMap
    {
        public PropertyMap()
        {
            // Space
        }

        public string Name { get; set; }

        public string PropertyName { get; set; }

        public Type PropertyType { get; set; }

        public MethodInfo Getter { get; set; }

        public MethodInfo Setter { get; set; }

        public Type DeclaringType { get; set; }
    }
}
