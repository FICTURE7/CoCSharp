using System;

namespace CoCSharp.Csv
{
    internal sealed class CsvDataRowDebugView
    {
        public CsvDataRowDebugView(CsvDataRow collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            _collection = collection;
        }

        private readonly CsvDataRow _collection;

        public int ID => _collection.ID;
        public string Name => _collection.Name;
        public object[] Data => _collection.GetAllData();
    }
}
