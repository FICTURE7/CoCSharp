using System;
using System.Collections.Generic;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents Clash of Clans data from .csv file.
    /// </summary>
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
            _minId = KindId * InternalConstants.IdBase;
            _maxId = _minId + MaxIndex;
        }
        #endregion

        #region Fields & Properties
        internal CsvDataRowRef _ref;
        // Max data ID of the type.
        private readonly int _minId;
        // Min data ID of the type.
        private readonly int _maxId;

        [CsvIgnore]
        internal abstract int KindId { get; }

        /// <summary>
        /// Gets the <see cref="CsvDataRowRef"/> which references 
        /// </summary>
        [CsvIgnore]
        public CsvDataRowRef RowRef => _ref;

        /// <summary>
        /// 
        /// </summary>
        [CsvIgnore]
        public int Id => _ref == null ? -1 : _ref.Id;
        #endregion

        #region Methods
        // Returns a proper ArgumentOutOfRangeException.Message for the specified parameter name.
        internal string GetArgsOutOfRangeMessage(string paramName) => paramName + " must be between " + _minId + " and " + _maxId + ".";

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

        internal static int GetKindID(Type type)
        {
            var instance = (CsvData)null;
            if (!s_instances.TryGetValue(type, out instance))
            {
                instance = (CsvData)Activator.CreateInstance(type);
                s_instances.Add(type, instance);
            }
            return instance.KindId;
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
