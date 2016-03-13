using System;

namespace CoCSharp.Logic
{
    //TODO: Add more genericness for event handling. Like a LogicFinishEventArgs.

    /// <summary>
    /// Provides arguments data for construction finish event. 
    /// </summary>
    public class ConstructionFinishEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructionFinishEventArgs"/> class.
        /// </summary>
        public ConstructionFinishEventArgs()
        {
            // Space
        }

        private DateTime _endTime;

        /// <summary>
        /// Gets or sets the <see cref="Buildable"/> that was constructed.
        /// </summary>
        public Buildable BuildableConstructed { get; set; }

        /// <summary>
        /// Gets or sets when the construction of the <see cref="Buildable"/> was finished.
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

        /// <summary>
        /// Gets or sets whether the construction of the <see cref="Buildable"/> was cancelled,
        /// that is when <see cref="Buildable.CancelConstruction"/> is called.
        /// </summary>
        public bool WasCancelled { get; set; }

        /// <summary>
        /// Gets or sets the user token object associated with the <see cref="Buildable"/> that was
        /// constructed.
        /// </summary>
        public object UserToken { get; set; }
    }
}
