using System;
using System.Collections.Generic;

namespace CoCSharp.Csv
{
    public class CsvDataRowCollection<TCsvData> : CsvDataRowCollection, ICollection<CsvDataRow<TCsvData>> where TCsvData : CsvData, new()
    {
        public CsvDataRowCollection()
        {
            _rows = new List<CsvDataRow<TCsvData>>();
        }

        private readonly List<CsvDataRow<TCsvData>> _rows;

        bool ICollection<CsvDataRow<TCsvData>>.IsReadOnly => false;
        public override int Count => _rows.Count;

        public override void Add(CsvDataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            _rows.Add((CsvDataRow<TCsvData>)row);
        }

        public void Add(CsvDataRow<TCsvData> row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            _rows.Add(row);
        }

        public override bool Remove(CsvDataRow item)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(CsvDataRow item)
        {
            throw new NotImplementedException();
        }

        public override void CopyTo(CsvDataRow[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<CsvDataRow> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        void ICollection<CsvDataRow<TCsvData>>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<CsvDataRow<TCsvData>>.Contains(CsvDataRow<TCsvData> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<CsvDataRow<TCsvData>>.CopyTo(CsvDataRow<TCsvData>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        IEnumerator<CsvDataRow<TCsvData>> IEnumerable<CsvDataRow<TCsvData>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        bool ICollection<CsvDataRow<TCsvData>>.Remove(CsvDataRow<TCsvData> item)
        {
            throw new NotImplementedException();
        }
    }
}
