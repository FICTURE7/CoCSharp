using System;
using System.Diagnostics;
using System.Linq;

namespace CoCSharp.Csv
{
    internal sealed class CsvDataRowDebugView
    {
        public CsvDataRowDebugView(CsvDataRow row)
        {
            if (row == null)
                throw new ArgumentNullException("row");

            _row = row;
        }

        private readonly CsvDataRow _row;

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public object[] Columns => _row.ToArray();
    }
}
