using System;

namespace CoCSharp.Csv
{
    internal sealed class CsvDataRowDebugView
    {
        public CsvDataRowDebugView(CsvDataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            _row = row;
        }

        private readonly CsvDataRow _row;

        public int ID => _row.ID;
        public CsvDataRowRef Ref => _row.Ref;
        public string Name => _row.Name;
        public object[] Data => _row.GetAllData();
    }
}
