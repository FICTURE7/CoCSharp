using CoCSharp.Csv;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Provides arguments data for construction finish event. 
    /// </summary>
    [Obsolete]
    public class ConstructionEventArgs<TCsvData> : LogicEventArgs where TCsvData : CsvData, new()
    {
        internal ConstructionEventArgs(LogicOperation op, Buildable<TCsvData> buildable, DateTime endTime, object userToken) : base(op)
        {
            if (buildable == null)
                throw new ArgumentNullException("buildable");
            if (endTime.Kind != DateTimeKind.Utc)
                throw new ArgumentException("endTime.Kind must be in DateTimeKind.Utc.");

            UserToken = userToken;
            Buildable = buildable;
            Time = endTime;
        }

        /// <summary>
        /// Gets the user token object associated with the <see cref="Buildable{TCsvData}"/> that was
        /// constructed.
        /// </summary>
        public object UserToken { get; private set; }

        /// <summary>
        /// Gets the <see cref="Buildable{TCsvData}"/> that is associated with this
        /// <see cref="ConstructionEventArgs{TCsvData}"/>.
        /// </summary>
        public Buildable<TCsvData> Buildable { get; private set; }

        /// <summary>
        /// Gets when the <see cref="LogicOperation"/> was executed.
        /// </summary>
        public DateTime Time { get; private set; }
    }
}
