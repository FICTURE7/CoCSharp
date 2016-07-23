using CoCSharp.Csv;

namespace CoCSharp.Data
{
    /// <summary>
    /// Represents a Clash of Clans slot which is strongly associated to a <see cref="CsvData"/>.
    /// </summary>
    /// <typeparam name="TCsvData">
    /// <see cref="CsvData"/> to which the <see cref="Slot{TCsvData}"/> is associated with.
    /// </typeparam>
    public abstract class Slot<TCsvData> : Slot where TCsvData : CsvData, new()
    {
        //TODO: Add range check of slot id.

        internal Slot()
        {
            _instance = CsvData.GetInstance<TCsvData>();
        }

        internal TCsvData _instance;
    }
}
