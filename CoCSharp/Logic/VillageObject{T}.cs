using CoCSharp.Csv;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents an object in a <see cref="Village"/> which has a <see cref="CsvData"/> associated with it.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> associated with it.</typeparam>
    [DebuggerDisplay("ID = {ID}, DataRef = {DataRef}")]
    public abstract class VillageObject<TCsvData> : VillageObject where TCsvData : CsvData, new()
    {
        #region Constants
        // We're doing nothing with this.

        private static readonly PropertyChangedEventArgs s_dataChanged = new PropertyChangedEventArgs("Data");
        #endregion

        #region Constructors
        // Constructors that FromJsonReader method is going to use.
        internal VillageObject(Village village) : base(village)
        {
            // Space
        }


        internal VillageObject(Village village, CsvDataCollectionRef<TCsvData> dataRef) : base(village)
        {
            if (dataRef == null)
                throw new ArgumentNullException("dataRef");

            var table = Assets.Get<CsvDataTable>();
            _dataRef = dataRef;
            _data = dataRef.Get(table)[0];
        }

        internal VillageObject(Village village, CsvDataCollectionRef<TCsvData> dataRef, int x, int y) : base(village, x, y)
        {
            if (dataRef == null)
                throw new ArgumentNullException("dataRef");

            var table = Assets.Get<CsvDataTable>();
            _dataRef = dataRef;
            _data = dataRef.Get(table)[0];
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets the cache to the sub-collection in which Data is found.
        /// </summary>
        public CsvDataCollection<TCsvData> CollectionCache { get; protected set; }

        private readonly CsvDataCollectionRef<TCsvData> _dataRef;
        public CsvDataCollectionRef<TCsvData> DataRef
        {
            get
            {
                return _dataRef;
            }
        }

        internal TCsvData _data;
        /// <summary>
        /// Gets the <typeparamref name="TCsvData"/> associated with this <see cref="VillageObject"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// In <see cref="Buildable{TCsvData}"/> this value can be null when 
        /// <see cref="Buildable{TCsvData}.Level"/> is -1 or <see cref="Buildable{TCsvData}.NotConstructedLevel"/>.
        /// </remarks>
        public TCsvData Data
        {
            get
            {
                return _data;
            }
        }

        internal override void ResetVillageObject()
        {
            base.ResetVillageObject();

            _data = default(TCsvData);
            CollectionCache = default(CsvDataCollection<TCsvData>);
        }
        #endregion
    }
}
