using System;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    internal sealed class CsvDataTableDebugView
    {
        public CsvDataTableDebugView(CsvDataTable row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            _row = row;
        }

        private readonly CsvDataTable _row;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public object[] Columns => _row.GetAllColumns();
    }
}
