using System;

namespace CoCSharp.Logic
{
    // Experimenting with stuff here.

    /// <summary>
    /// Represents logic operations that can made on <see cref="VillageObject"/>.
    /// </summary>
    [Flags]
    public enum LogicOperation : int
    {
        /// <summary>
        /// An empty operation.
        /// </summary>
        None = 0,

        /// <summary>
        /// A starting operation.
        /// </summary>
        Started = 2,

        /// <summary>
        /// A finished operation.
        /// </summary>
        Finished = 4,

        /// <summary>
        /// A canceling operation.
        /// </summary>
        Cancel = 8,

        /// <summary>
        /// A speeding up operation.
        /// </summary>
        SpeedUp = 16,

        /// <summary>
        /// A buying operation.
        /// </summary>
        Buy = 32,

        /// <summary>
        /// An upgrading operation.
        /// </summary>
        Upgrade = 64,

        /// <summary>
        /// A selling operation.
        /// </summary>
        Sell = 128,

        /// <summary>
        /// A clearing operation.
        /// </summary>
        Clear = 256
    }
}
