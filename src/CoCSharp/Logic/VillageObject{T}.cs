using CoCSharp.Csv;
using System;
using System.Diagnostics;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents an object in a <see cref="Village"/> which has a <see cref="CsvData"/> associated with it.
    /// </summary>
    /// 
    /// <remarks>
    /// This class inherits from the <see cref="VillageObject"/> class.
    /// </remarks>
    /// 
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> associated with it.</typeparam>
    public abstract class VillageObject<TCsvData> : VillageObject where TCsvData : CsvData, new()
    {
        #region Constructors
        // Constructor used to load the VillageObject from a JsonTextReader.
        internal VillageObject() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VillageObject{TCsvData}"/> class with the specified
        /// <see cref="Village"/> instance and <typeparamref name="TCsvData"/>.
        /// </summary>
        /// 
        /// <param name="village">
        /// <see cref="Village"/> instance which owns this <see cref="VillageObject{TCsvData}"/>.
        /// </param>
        /// 
        /// <param name="data"><typeparamref name="TCsvData"/> representing the data of the <see cref="VillageObject{TCsvData}"/>.</param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="village"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        protected VillageObject(Village village, TCsvData data) : base(village)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(village));

            _data = data;
        }
        #endregion

        #region Fields & Properties
        // Data associated with this VillageObject.
        // Should never be = null unless the object has been reset.
        internal TCsvData _data;

        /// <summary/>
        protected override string DebuggerDisplayString => $"ID = {Id} Data = {(_data.RowRef == null ? "null" : _data.Id.ToString())}";

        /// <summary>
        /// Gets the <typeparamref name="TCsvData"/> associated with this <see cref="VillageObject"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// In <see cref="Buildable{TCsvData}"/> this value can be null when 
        /// <see cref="Buildable{TCsvData}.UpgradeLevel"/> is -1 or <see cref="Buildable{TCsvData}.NotConstructedLevel"/>.
        /// </remarks>
        public TCsvData Data => _data;

        /// <summary/>
        protected internal override void ResetVillageObject()
        {
            base.ResetVillageObject();

            _data = default(TCsvData);
        }
        #endregion
    }
}
