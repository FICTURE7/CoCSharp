using System;
using System.Collections.Generic;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a collection of <see cref="CsvDataColumnCollection{TCsvData}"/>.
    /// </summary>
    /// <typeparam name="TCsvData"></typeparam>
    public class CsvDataColumnCollection<TCsvData> : CsvDataColumnCollection, ICollection<CsvDataColumn<TCsvData>> where TCsvData : CsvData, new()
    {
        internal CsvDataColumnCollection() : base(typeof(TCsvData))
        {
            // Space
        }

        #region Fields & Properties
        bool ICollection<CsvDataColumn<TCsvData>>.IsReadOnly => false;

        public override int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Methods
        public override void Add(CsvDataColumn column)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override bool Contains(CsvDataColumn column)
        {
            throw new NotImplementedException();
        }

        public override void CopyTo(CsvDataColumn[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<CsvDataColumn> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override bool Remove(CsvDataColumn column)
        {
            throw new NotImplementedException();
        }

        public void Add(CsvDataColumn<TCsvData> item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(CsvDataColumn<TCsvData> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(CsvDataColumn<TCsvData>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(CsvDataColumn<TCsvData> item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<CsvDataColumn<TCsvData>> IEnumerable<CsvDataColumn<TCsvData>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
