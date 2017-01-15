using System;
using System.Data;
using System.Reflection;

namespace CoCSharp.Csv
{
    internal struct PropertyMap
    {
        public string Name;

        public DataColumn Column;

        public int ColumnIndex;

        public string PropertyName;

        public Type PropertyType;

        public MethodInfo Getter;

        public MethodInfo Setter;

        public Type DeclaringType;
    }
}
