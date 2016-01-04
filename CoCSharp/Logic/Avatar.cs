using CoCSharp.Networking;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans avatar.
    /// </summary>
    public class Avatar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Avatar"/> class.
        /// </summary>
        public Avatar()
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the username of the <see cref="Avatar"/>.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the user token of the <see cref="Avatar"/>.
        /// </summary>
        public string UserToken { get; set; }
        /// <summary>
        /// Gets or sets the user ID of the <see cref="Avatar"/>.
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// Gets or sets the shield duration of the <see cref="Avatar"/>.
        /// </summary>
        public TimeSpan ShieldDuration { get; set; }
    }
}
