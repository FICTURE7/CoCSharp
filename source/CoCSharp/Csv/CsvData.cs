using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a Clash of Clans .csv file.
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
            _minId = KindID * InternalConstants.IDBase;
            _maxId = _minId + MaxIndex;
        }
        #endregion

        #region Fields & Properties
        // Max data ID of the type.
        private readonly int _minId;
        // Min data ID of the type.
        private readonly int _maxId;

        [CsvIgnore]
        internal abstract int KindID { get; }

        internal CsvDataRowRef _ref;
        [CsvIgnore]
        public CsvDataRowRef RowRef
        {
            get
            {
                return _ref;
            }
        }

        public int ID
        {
            get
            {
                return _ref.ID;
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
            var type = typeof(TCsvData);
            var instance = (CsvData)null;
            if (!s_instances.TryGetValue(type, out instance))
            {
                instance = new TCsvData();
                s_instances.Add(type, instance);
            }

            return (TCsvData)instance;
        }

        private static CsvData GetInstance(Type type)
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

        internal static int GetKindID(Type type)
        {
            Debug.Assert(type.IsAbstract || type.BaseType != typeof(CsvData));

            var instance = (CsvData)null;
            if (!s_instances.TryGetValue(type, out instance))
            {
                instance = (CsvData)Activator.CreateInstance(type);
                s_instances.Add(type, instance);
            }
            return instance.KindID;
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
