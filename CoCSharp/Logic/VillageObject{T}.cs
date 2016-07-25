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
    [DebuggerDisplay("ID = {ID}, Data.TID = {Data.TID}")]
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


        internal VillageObject(Village village, TCsvData data) : base(village)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            _data = data;
        }

        internal VillageObject(Village village, TCsvData data, int x, int y) : base(village, x, y)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            _data = data;
        }
        #endregion

        #region Fields & Properties
        // Level of the construction.
        internal int _constructionLevel;

        ///<summary>Cache to the sub-collection in which Data is found.</summary>
        protected CsvDataSubCollection<TCsvData> CollectionCache { get; set; }

        internal TCsvData _data;
        /// <summary>
        /// Gets or sets the <typeparamref name="TCsvData"/> associated with this <see cref="VillageObject"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public TCsvData Data
        {
            get
            {
                return _data;
            }
            set
            {
                //NOTE: Changing this might break the UpdateCanUpgrade() method.

                if (value == null)
                    throw new ArgumentNullException("value");

                if (_data == value)
                    return;

                // We have changed CsvDataSubCollection.
                if (_data.ID != value.ID)
                    CollectionCache = null;

                _constructionLevel = value.Level;
                _data = value;
                OnPropertyChanged(s_dataChanged);
            }
        }
        #endregion
    }
}
