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

        /// <summary>
        /// Gets or sets the <see cref="Buildable"/> that was constructed.
        /// </summary>
        public Buildable BuildableConstructed { get; set; }

        /// <summary>
        /// Gets or sets when the construction of the <see cref="Buildable"/> was finished.
        /// </summary>
        public DateTime EndTime { get; set; } //TODO: Check if its kind is of Utc.

        /// <summary>
        /// Gets or sets whether the construction of the <see cref="Buildable"/> was cancelled,
        /// that is when <see cref="Buildable.EndConstruction"/> is called.
        /// </summary>
        public bool WasCancelled { get; set; }

        /// <summary>
        /// Gets or sets the user token object associated with the <see cref="Buildable"/> that was
        /// constructed.
        /// </summary>
        public object UserToken { get; set; }
    }
}
