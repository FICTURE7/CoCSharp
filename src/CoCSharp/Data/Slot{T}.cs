using CoCSharp.Csv;

namespace CoCSharp.Data
{
    /// <summary>
    /// Represents a Clash of Clans slot which is strongly associated to a <see cref="CsvData"/> and
    /// refers to it.
    /// </summary>
    /// 
    /// <typeparam name="TCsvData">
    /// <see cref="CsvData"/> to which the <see cref="Slot{TCsvData}"/> is associated with.
    /// </typeparam>
    public abstract class Slot<TCsvData> : Slot where TCsvData : CsvData, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Slot{TCsvData}"/> class.
        /// </summary>
        protected Slot()
        {
            _instance = CsvData.GetInstance<TCsvData>();
        }

        private readonly TCsvData _instance;

        internal override bool InvalidDataId(int dataId)
        {
            return _instance.InvalidDataID(dataId);
        }

        internal override string GetArgsOutOfRangeMessage(string paramName)
        {
            return _instance.GetArgsOutOfRangeMessage(paramName);
        }
    }
}
