using CoCSharp.Csv;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Data.AssetProviders
{
    // Asset loader for loading .csv tables.
    internal class CsvDataTableAssetProvider : AssetProvider
    {
        public CsvDataTableAssetProvider()
        {
            Table = new CsvDataTableCollection();
            _type2id = new Dictionary<Type, int>();
        }

        private bool _disposed;
        // Set of CsvDataTable where we store the loaded CsvDataTables.
        public readonly CsvDataTableCollection Table;

        // Dictionary mapping types of loaded CsvData to their KindIDs.
        private readonly Dictionary<Type, int> _type2id;

        // Loads the specified CsvDataTable<TCsvData> type at the specified path, where type must be a generic.
        public override void LoadAsset(Type type, string path)
        {
            // Make sure its a generic type.
            Debug.Assert(type.IsGenericType, "type should be generic.");

            // Use the generic argument in type, as the arguments
            // in CsvConvert.Deserialize(CsvTable, Type).
            // The genericArgs should be of type CsvData.
            var genericArgs = type.GetGenericArguments()[0];

            Debug.Assert(genericArgs.BaseType == typeof(CsvData) && !genericArgs.IsAbstract);

            // Load .csv file into a CsvTable object.
            var table = new CsvTable(path);

            // Convert that CsvTable object into a CsvDataTable<TCsvData> object.
            var dataTable = CsvConvert.Deserialize(table, genericArgs);
            Table.Add(dataTable);

            _type2id.Add(type, dataTable.KindID);
        }

        public override bool UnloadAsset(Type type)
        {
            return _type2id.Remove(type);
        }

        // Returns the CsvDataTable<> of the specified type.
        public override object GetAsset(Type type)
        {
            if (type == typeof(CsvDataTableCollection))
                return Table;

            // Get the Kind ID from type.
            var id = default(int);
            if (!_type2id.TryGetValue(type, out id))
                return null;

            return Table[id];
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Remove refs to tables.
                _type2id.Clear();
            }

            base.Dispose(disposing);
            _disposed = true;
        }
    }
}
