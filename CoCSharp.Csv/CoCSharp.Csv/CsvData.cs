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

            _ref = null;
            _level = -1;

            _minId = KindID * InternalConstants.IDBase;
            _maxId = _minId + MaxIndex;
            _argsOutOfRangeMessage = " must be between " + _minId + " and " + _maxId + ".";
        }
        #endregion

        #region Fields & Properties
        // Max data ID of the type.
        private readonly int _minId;
        // Min data ID of the type.
        private readonly int _maxId;

        // A KindID of 0 will break the maths behind Data IDs.
        [CsvIgnore]
        internal abstract int KindID { get; }

        internal CsvDataCollectionRef _ref;
        [CsvIgnore]
        public CsvDataCollectionRef CollectionRef
        {
            get
            {
                return _ref;
            }
        }

        [CsvIgnore]
        public int ID
        {
            get
            {
                if (_ref == null)
                    throw new InvalidOperationException("CsvData must be in a CsvDataCollection to have an ID and a Level.");

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

                if (_level == -1)
                {
                    Debug.Assert(_ref == null, "_ref was not null but _level was -1");
                    throw new InvalidOperationException("CsvData must be in a CsvDataCollection to have an ID and a Level.");
                }

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

        // Returns an instance of the specified type.
        // To prevent extra creation of objects.
        internal static TCsvData GetInstance<TCsvData>() where TCsvData : CsvData, new()
        {
            var type = typeof(TCsvData);
            var instance = (CsvData)null;
            if (!s_instances.TryGetValue(type, out instance))
            {
                instance = new TCsvData();
                s_instances.Add(type, instance);
            }

            return (TCsvData)instance;
        }

        internal static CsvData GetInstance(Type type)
        {
            if (type.IsAbstract || type.BaseType != typeof(CsvData))
                throw new Exception();

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
