using System;
using System.Linq;

namespace CoCSharp.Csv
{
    internal sealed class CsvDataCollectionDebugView
    {
        public CsvDataCollectionDebugView(CsvDataCollection collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            _collection = collection;
        }

        private readonly CsvDataCollection _collection;

        public int ID => _collection.ID;

        public string Name => _collection.Name;

        public object[] Items => _collection.ToArray();
    }
}
