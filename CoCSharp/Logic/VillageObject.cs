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
        //TODO: Make the constructor internal.

        // Represents the Base ID of every game ID & data ID.
        internal const int Base = 1000000;

        internal VillageObject()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VillageObject"/> class with
        /// the specified X coordinate and Y coordinate.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="x"/> is not between 0 and Village.Width.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="y"/> is not between 0 and Village.Height.</exception>
        public VillageObject(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets or sets the X coordinate of the <see cref="VillageObject"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not between 0 and Village.Width.</exception>
        public int X
        {
            get { return _x; }
            set
            {
                if (value < 0 || value > Village.Width)
                    throw new ArgumentOutOfRangeException("value", "value must be between 0 and Village.Width.");

                _x = value;
            }
        }

        // X coordinate of object.
        private int _x;

        /// <summary>
        /// Gets or sets the y coordinate of the <see cref="VillageObject"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not between 0 and Village.Height.</exception>
        public int Y
        {
            get { return _y; }
            set
            {
                if (value < 0 || value > Village.Height)
                    throw new ArgumentOutOfRangeException("value", "value must be between 0 and Village.Height.");

                _y = value;
            }
        }

        // Y coordinate of object.
        private int _y;

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
                // Update _dataID with the provided data.
                if (value != null)
                {
                    var type = value.GetType();

                    // Make sure its of the expected type.
                    if (type != ExpectedDataType)
                        throw new ArgumentException("Expected value to in the type of '" + ExpectedDataType + "'.", "value");

                    _dataId = value.ID;
                }
                else
                    _dataId = 0;

                _data = value;
            }
        }

        // Data associated with the object.
        private CsvData _data;

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
        // ID of _data;
        private int _dataId;

        //TODO: Turn this into an abstract function.

        /// <summary>
        /// Gets the <see cref="Type"/> of the <see cref="CsvData"/> expected
        /// by the <see cref="VillageObject"/>.
        /// </summary>
        protected abstract Type ExpectedDataType { get; }

        /// <summary>
        /// Returns the data ID of the <see cref="CsvData"/> associated with the <see cref="VillageObject"/>.
        /// </summary>
        /// <returns>Data ID of the <see cref="CsvData"/> associated with the <see cref="VillageObject"/>.</returns>
        public int GetDataID()
        {
            //TODO: Needs some API improvement because it can accessed from Data.ID & here
            // however Data can be null.

            return _dataId;
        }

        internal virtual void ToJsonWriter(JsonWriter writer)
        {
            throw new NotImplementedException();
        }

        internal virtual void FromJsonReader(JsonReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
