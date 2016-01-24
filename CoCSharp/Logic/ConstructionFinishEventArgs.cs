using System;

namespace CoCSharp.Logic
{
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
        /// Gets or sets the <see cref="Logic.Building"/> that was constructed.
        /// </summary>
        public Building Building { get; set; }

        /// <summary>
        /// Gets or sets when the construction of the <see cref="Logic.Building"/> was finished.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets whether the construction of the <see cref="Logic.Building"/> was ended.
        /// That is when <see cref="Logic.Building.EndConstruct"/> is called.
        /// </summary>
        public bool WasEnded { get; set; }
    }
}
