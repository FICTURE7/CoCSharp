using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Provides arguments data for clearing finish event.
    /// </summary>
    public class ClearingFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearingFinishedEventArgs"/> class.
        /// </summary>
        public ClearingFinishedEventArgs()
        {
            // Space
        }        

        /// <summary>
        /// Gets or sets the <see cref="Obstacle"/> that was cleared.
        /// </summary>
        public Obstacle ClearedObstacle { get; set; }

        private DateTime _endTime;
        /// <summary>
        /// Gets or sets the UTC time at which the obstacle clearing operation was finished.
        /// </summary>
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
        /// Gets or sets whether the clearing of the <see cref="Obstacle"/> was cancelled,
        /// that is when <see cref="Obstacle.CancelClearing"/> is called.
        /// </summary>
        public bool WasCancelled { get; set; }

        /// <summary>
        /// Gets or sets the user token object associated with the <see cref="Obstacle"/> that was
        /// cleared.
        /// </summary>
        public object UserToken { get; set; }
    }
}
