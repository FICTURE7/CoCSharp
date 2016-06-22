using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;

namespace CoCSharp.Data
{
    //TODO: Figure something better for it.

    /// <summary>
    /// Represents data about move village object sent
    /// in <see cref="MoveVillageObjectCommand"/> and <see cref="MoveMultipleVillageObjectCommand"/>.
    /// </summary>
    public class MoveVillageObjectMessageComponent : MessageComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveVillageObjectMessageComponent"/> class.
        /// </summary>
        public MoveVillageObjectMessageComponent()
        {
            // Space
        }

        /// <summary>
        /// X coordinates of the new position.
        /// </summary>
        public int X;
        /// <summary>
        /// Y coordinates of the new position.
        /// </summary>
        public int Y;
        /// <summary>
        /// <see cref="VillageObject"/> game ID in a <see cref="Village"/> that
        /// was moved.
        /// </summary>
        public int VillageObjectGameID;
    }
}
