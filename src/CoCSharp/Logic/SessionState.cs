using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Defines value indicating the state of a <see cref="Session"/>.
    /// </summary>
    [Flags]
    public enum SessionState
    {
        /// <summary>
        /// <see cref="Session"/> is in its default state.
        /// </summary>
        None = 0,

        /// <summary>
        /// <see cref="Session"/> is logged in with a <see cref="Level"/>.
        /// </summary>
        LoggedIn = 1,
    }
}
