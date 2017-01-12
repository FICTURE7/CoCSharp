using MySql.Data.MySqlClient;
using System.Data;

namespace CoCSharp.Server.Db
{
    internal static class MySqlCommandExtensions
    {
        public static void AddWithValue(this MySqlParameterCollection collection, string paramterName, object value)
        {
            var para = new MySqlParameter()
            {
                Value = value,
                ParameterName = paramterName
            };

            var index = collection.Add(para);
        }
    }
}
