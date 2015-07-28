using System;

namespace CoCSharp.Data.Csv
{
    public class CsvSerializer
    {
        public static object Deserialize(CsvTable table, Type objectType)
        {
            var value = Activator.CreateInstance(objectType);
            
            return value;
        }
    }
}
