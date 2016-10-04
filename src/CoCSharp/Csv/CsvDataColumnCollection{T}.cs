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
        internal CsvDataColumnCollection(CsvDataTable table) : base(typeof(TCsvData), table)
        {
            _columns = new List<CsvDataColumn>(16);
        }

        #region Fields & Properties
        private readonly List<CsvDataColumn> _columns;

        bool ICollection<CsvDataColumn<TCsvData>>.IsReadOnly => false;

        /// <summary>
        /// 
        /// </summary>
        public override int Count => _columns.Count;
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        public override void Add(CsvDataColumn column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        public void Add(CsvDataColumn<TCsvData> column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public override bool Contains(CsvDataColumn column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));
                
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public override bool Remove(CsvDataColumn column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool Remove(CsvDataColumn<TCsvData> column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool Contains(CsvDataColumn<TCsvData> column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Clear()
        {
            _columns.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(CsvDataColumn<TCsvData>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public override void CopyTo(CsvDataColumn[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<CsvDataColumn> GetEnumerator()
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
