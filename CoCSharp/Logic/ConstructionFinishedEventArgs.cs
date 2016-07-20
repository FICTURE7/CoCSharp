using CoCSharp.Csv;
using System;

namespace CoCSharp.Logic
{
    //TODO: Add more genericness for event handling. Like a LogicFinishEventArgs.

    /// <summary>
    /// Provides arguments data for construction finish event. 
    /// </summary>
    public class ConstructionFinishedEventArgs<TCsvData> : EventArgs where TCsvData : CsvData, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructionFinishedEventArgs{TCsvData}"/> class.
        /// </summary>
        public ConstructionFinishedEventArgs()
        {
            // Space
        }

        private DateTime _endTime;

        /// <summary>
        /// Gets or sets the <see cref="Buildable{TCsvData}"/> that was constructed.
        /// </summary>
        public Buildable<TCsvData> BuildableConstructed { get; set; }

        /// <summary>
        /// Gets or sets when the construction of the <see cref="Buildable{TCsvData}"/> was finished.
        /// </summary>
        /// <remarks>
        /// The <see cref="DateTimeKind"/> of the value specified must of <see cref="DateTimeKind.Utc"/>.
        /// </remarks>
        /// <exception cref="ArgumentException"><paramref name="value"/> kind must a kind of <see cref="DateTimeKind.Utc"/>.</exception>
        public DateTime EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                if (value.Kind != DateTimeKind.Utc)
                    throw new ArgumentException("EndTime.Kind must be in DateTimeKind.Utc.", "value");

                _endTime = value;
            }
        }

        //TODO: Implement an enum for this instead.
        /// <summary>
        /// Gets or sets whether the construction of the <see cref="Buildable{TCsvData}"/> was cancelled,
        /// that is when <see cref="Buildable{TCsvData}.CancelConstruction"/> is called.
        /// </summary>
        public bool WasCancelled { get; set; }

        /// <summary>
        /// Gets or sets the user token object associated with the <see cref="Buildable{TCsvData}"/> that was
        /// constructed.
        /// </summary>
        public object UserToken { get; set; }
    }
}
