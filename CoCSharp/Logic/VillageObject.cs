using CoCSharp.Csv;
using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents an object in a <see cref="Village"/>.
    /// </summary>
    public abstract class VillageObject
    {
        // Represents the Base ID of every game ID & data ID.
        internal const int Base = 1000000;

        #region Constructors
        internal VillageObject(Village village)
        {
            _village = village;
        }

        internal VillageObject(Village village, int x, int y) : this(village)
        {
            X = x;
            Y = y;
        }
        #endregion

        #region Fields & Properties
        // Village in which the VillageObject is in.
        private Village _village;
        /// <summary>
        /// Gets the <see cref="Logic.Village"/> in which the current <see cref="VillageObject"/> is in.
        /// </summary>
        public Village Village
        {
            get
            {
                return _village;
            }
        }

        // X coordinate of object.
        private int _x;
        /// <summary>
        /// Gets or sets the X coordinate of the <see cref="VillageObject"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not between 0 and Village.Width.</exception>
        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                if (value < 0 || value > Village.Width)
                    throw new ArgumentOutOfRangeException("value", "value must be between 0 and Village.Width.");

                _x = value;
            }
        }

        // Y coordinate of object.
        private int _y;
        /// <summary>
        /// Gets or sets the y coordinate of the <see cref="VillageObject"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not between 0 and Village.Height.</exception>
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                if (value < 0 || value > Village.Height)
                    throw new ArgumentOutOfRangeException("value", "value must be between 0 and Village.Height.");

                _y = value;
            }
        }

        // Data associated with the object.
        private CsvData _data;
        /// <summary>
        /// Gets or sets the <see cref="CsvData"/> associated with the <see cref="VillageObject"/>.
        /// </summary>
        /// <remarks>
        /// This is needed for handling of construction, upgrading and other game logic actions. It must be updated
        /// regularly to keep up with the current game state.
        /// </remarks>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not type of expected type.</exception>
        public CsvData Data
        {
            get
            {
                return _data;
            }
            set
            {
                // Update _dataId with the provided data.
                if (value != null)
                {
                    var type = value.GetType();

                    // Make sure its of the expected type.
                    if (type != ExpectedDataType)
                        throw new ArgumentException("Expected value to in the type of '" + ExpectedDataType + "'.", "value");

                    _dataId = value.ID;
                }
                else
                {
                    _dataId = 0;
                }

                _data = value;
            }
        }

        // ID of _data;
        private int _dataId;
        internal int DataID
        {
            get
            {
                return _dataId;
            }
            set
            {
                _dataId = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the <see cref="CsvData"/> expected
        /// by the <see cref="VillageObject"/>.
        /// </summary>
        protected abstract Type ExpectedDataType { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the data ID of the <see cref="CsvData"/> associated with the <see cref="VillageObject"/>.
        /// </summary>
        /// <returns>Data ID of the <see cref="CsvData"/> associated with the <see cref="VillageObject"/>.</returns>
        public int GetDataID()
        {
            //TODO: Needs some API improvement because it can accessed from Data.ID & here
            // however Data can be null. Could be improved by creating a NullData object.

            return _dataId;
        }

        // Writes the current VillageObject to the JsonWriter.
        internal virtual void ToJsonWriter(JsonWriter writer)
        {
            throw new NotImplementedException();
        }

        // Reads the current VillageObject to the JsonReader.
        internal virtual void FromJsonReader(JsonReader reader)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
