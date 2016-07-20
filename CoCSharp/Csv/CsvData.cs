using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a Clash of Clans .csv file.
    /// </summary>
    [DebuggerDisplay("TID = {TID}, ID = {ID}, Level = {Level}")]
    public abstract class CsvData
    {
        #region Constants
        // Contains instances of TCsvData.
        // To reduce the amount of _instance object duplicates.
        internal static Dictionary<Type, WeakReference> s_instances = new Dictionary<Type, WeakReference>();

        internal const int MaxIndex = 999999;
        #endregion

        #region Constructors
        internal CsvData()
        {
            _minId = BaseDataID;
            _maxId = BaseDataID + MaxIndex;
        }
        #endregion

        #region Fields & Properties
        // Determines if the CsvData object is in a CsvDataSubCollection.
        // We don't want them to modify the TID, ID and Level of the CsvData after they added it to the collection.
        internal bool _isInCollection;

        // Max data ID of the type.
        private readonly int _minId;
        // Min data ID of the type.
        private readonly int _maxId;

        // Base data ID, Example: 1000000 for BuildingData
        // Uses this to provide better error messages and ID checks.
        [CsvIgnore]
        internal abstract int BaseDataID { get; }

        /// <summary>
        /// Gets or sets the data ID of the <see cref="CsvData"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="CsvData"/> is in a <see cref="CsvDataSubCollection{TCsvData}"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not within the range of the <see cref="CsvData"/>'s ID.</exception>
        [CsvIgnore]
        public int ID
        {
            get
            {
                return Index + BaseDataID;
            }
            set
            {
                // Prevent user from modifying a CsvData after adding it to a CsvDataSubCollection.
                if (_isInCollection)
                    throw new InvalidOperationException("Cannot modify ID, TID or Level when it is in a CsvDataSubCollection.");

                if (InvalidDataID(value))
                    throw new ArgumentOutOfRangeException("value", GetArgsOutOfRangeMessage("value"));

                Index = value - BaseDataID;
            }
        }

        private string _tid;
        /// <summary>
        /// Gets or sets the text ID of the <see cref="CsvData"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="CsvData"/> is in a <see cref="CsvDataSubCollection{TCsvData}"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public string TID
        {
            get
            {
                return _tid;
            }
            set
            {
                // Prevent user from modifying a CsvData after adding it to a CsvDataSubCollection.
                if (_isInCollection)
                    throw new InvalidOperationException("Cannot modify ID, TID or Level when it is in a CsvDataSubCollection.");

                if (value == null)
                    throw new ArgumentNullException("value");

                _tid = value;
            }
        }

        private int _level;
        /// <summary>
        /// Gets or sets the level of <see cref="CsvData"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="CsvData"/> is in a <see cref="CsvDataSubCollection{TCsvData}"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        [CsvIgnore]
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                // Prevent user from modifying a CsvData after adding it to a CsvDataSubCollection.
                if (_isInCollection)
                    throw new InvalidOperationException("Cannot modify ID, TID or Level when it is in a CsvDataSubCollection.");

                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "Level must be non-negative.");

                _level = value;
            }
        }

        private int _index;
        // Index of the CsvData in the CSV file. Aka parent ID.
        [CsvIgnore]
        internal int Index
        {
            get
            {
                return _index;
            }
            set
            {
                // This exception can occur if there is more than 1000000 child object.
                if (value < 0 || value > MaxIndex)
                    throw new ArgumentOutOfRangeException("value");

                _index = value;
            }
        }
        #endregion

        #region Methods
        private string _argsOutOfRangeMessage;
        // Returns a proper ArgumentOutOfRangeException.Message for the specified parameter name.
        internal string GetArgsOutOfRangeMessage(string paramName)
        {
            if (_argsOutOfRangeMessage == null)
                _argsOutOfRangeMessage = " must be between " + _minId + " and " + _maxId + ".";

            return paramName + _argsOutOfRangeMessage;
        }

        // Returns an instance of the specified type.
        // To prevent extra creation of objects.
        internal static TCsvData GetInstance<TCsvData>() where TCsvData : CsvData, new()
        {
            var instance = (TCsvData)null;
            var weakRef = (WeakReference)null;
            // If we don't have any weak references to the TCsvData instance,
            // we create a new instance and add its weak reference to the dictionary.
            if (!s_instances.TryGetValue(typeof(TCsvData), out weakRef))
            {
                instance = new TCsvData();

                weakRef = new WeakReference(instance);
                s_instances.Add(typeof(TCsvData), weakRef);
            }
            else
            {
                // If we don't manage to get the weak reference's target object
                // we create a new instance and update the weak reference.
                // Likely to happen when the GC kicked in.
                if (!weakRef.IsAlive)
                {
                    instance = new TCsvData();
                    weakRef.Target = instance;
                }
                else
                {
                    instance = (TCsvData)weakRef.Target;
                }
            }

            return instance;
        }

        internal int GetIndex(int dataId)
        {
            return dataId - _minId;
        }

        // Determines if the specified dataId is invalid (not between valid range).
        internal bool InvalidDataID(int dataId)
        {
            return dataId < _minId || dataId > _maxId;
        }
        #endregion
    }
}
