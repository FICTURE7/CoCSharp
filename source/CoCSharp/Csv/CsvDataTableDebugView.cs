using System;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    internal sealed class CsvDataTableDebugView
    {
        public CsvDataTableDebugView(CsvDataTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _table = table;
        }

        private readonly CsvDataTable _table;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public object[] Rows => _table.GetAllColumns();
    }
}
