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
        internal readonly int _baseDataId;

        private static readonly PropertyChangedEventArgs s_dataChanged = new PropertyChangedEventArgs("Data");
        #endregion

        #region Constructors
        // Constructors that FromJsonReader method is going to use.
        internal VillageObject(Village village) : base(village)
        {
            _baseDataId = CsvData.GetInstance<TCsvData>().BaseDataID;
        }


        internal VillageObject(Village village, TCsvData data) : base(village)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            _data = data;
            //CheckAndSetTownHall();

            _baseDataId = CsvData.GetInstance<TCsvData>().BaseDataID;
        }

        internal VillageObject(Village village, TCsvData data, int x, int y) : base(village, x, y)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            _data = data;
            //CheckAndSetTownHall();

            _baseDataId = CsvData.GetInstance<TCsvData>().BaseDataID;
        }
        #endregion

        #region Fields & Properties
        // Cache to the collection in which Data is found.
        internal CsvDataSubCollection<TCsvData> _collectionCache;

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
                if (value == null)
                    throw new ArgumentNullException("value");

                if (_data == value)
                    return;
                if (_data.ID != value.ID)
                    _collectionCache = null;

                OnPropertyChanged(s_dataChanged);
                _data = value;
            }
        }
        #endregion

        #region Methods
        // Determines if the current VillageObject is a TownHall building based on Data.TID
        // and set the townhall of the Village to this VillageObject. 
        internal void CheckAndSetTownHall()
        {           
            if (Data.TID == "TID_BUILDING_TOWN_HALL")
            {
                // A Village cannot contain more than 1 townhall.
                if (Village._townhall != null)
                    throw new InvalidOperationException("Village already contains a TownHall.");

                Village._townhall = this;
            }
        }
        #endregion
    }
}
