using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a Clash of Clans .csv file.
    /// </summary>
    [DebuggerDisplay("Level = {Level}")]
    public abstract class CsvData
    {
        #region Constants
        // Contains instances of TCsvData.
        // To reduce the amount of _instance object duplicates.
        internal static readonly Dictionary<Type, CsvData> s_instances = new Dictionary<Type, CsvData>();

        internal const int MaxIndex = 999999;
        #endregion

        #region Constructors
        internal CsvData()
        {
            // Should not really happen unless, the implementation messed up.
            if (KindID <= 0)
                throw new Exception("Invalid implementation, KindID must be greater than 0.");

            _ref = CsvDataCollectionRef.NullRef;
            _level = -1;

            _minId = KindID * InternalConstants.IDBase;
            _maxId = _minId + MaxIndex;
            _argsOutOfRangeMessage = " must be between " + _minId + " and " + _maxId + ".";
        }
        #endregion

        #region Fields & Properties
        internal CsvDataCollectionRef _ref;
        // Max data ID of the type.
        private readonly int _minId;
        // Min data ID of the type.
        private readonly int _maxId;

        // A KindID of 0 will break the maths behind Data IDs.
        [CsvIgnore]
        internal abstract int KindID { get; }

        [CsvIgnore]
        public CsvDataCollectionRef CollectionRef => _ref;

        [CsvIgnore]
        public int ID
        {
            get
            {
                if (_ref == CsvDataCollectionRef.NullRef)
                    throw new InvalidOperationException("CsvData must be in a CsvDataCollection which itself in a CsvDataRow to have an ID and a Level.");

                return _ref.ID;
            }
        }

        // Index of CsvData in a CsvDataCollection,
        // Also the Level of the CsvData.
        internal int _level;
        /// <summary>
        /// Gets or sets the level of <see cref="CsvData"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="CsvData"/> is not in a <see cref="CsvDataCollection{TCsvData}"/>.</exception>
        [CsvIgnore]
        public int Level
        {
            get
            {
                Debug.Assert(_level >= -1, "_level was less than -1.");
                if (_ref == CsvDataCollectionRef.NullRef)
                    throw new InvalidOperationException("CsvData must be in a CsvDataCollection which itself in a CsvDataRow to have an ID and a Level.");

                return _level;
            }
        }
        #endregion

        #region Methods
        private string _argsOutOfRangeMessage;
        // Returns a proper ArgumentOutOfRangeException.Message for the specified parameter name.
        internal string GetArgsOutOfRangeMessage(string paramName)
        {
            return paramName + _argsOutOfRangeMessage;
        }

        // To determine if we're inside a collection or not.
        private bool _inCollection;
        // Called when the CsvData is added to a CsvDataCollection.
        internal void OnAdd(CsvDataCollection dataCollection, int level)
        {
            _ref = dataCollection.CollectionRef;
            _level = level;
            _inCollection = true;
        }

        // Called when the CsvData is removed from a CsvDataCollection.
        internal void OnRemove()
        {
            _ref = CsvDataCollectionRef.NullRef;
            _level = -1;
            _inCollection = false;
        }

        internal static CsvData GetInstance(Type type)
        {
            var instance = (CsvData)null;
            if (!s_instances.TryGetValue(type, out instance))
            {
                instance = (CsvData)Activator.CreateInstance(type);
                s_instances.Add(type, instance);
            }
            return instance;
        }

        // Returns the index of data ID.
        // This value depends on BaseGameID.
        // E.g: 1000000 => 0
        //      5000004 => 4
        //      12000001 => 1
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
