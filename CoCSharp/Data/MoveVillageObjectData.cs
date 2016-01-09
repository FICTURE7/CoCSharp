using CoCSharp.Logic;
using CoCSharp.Networking.Messages.Commands;

namespace CoCSharp.Data
{
    /// <summary>
    /// Represents data about move village object sent
    /// in <see cref="MoveVillageObjectCommand"/> and <see cref="MoveMultipleVillageObjectCommand"/>.
    /// </summary>
    public class MoveVillageObjectData
    {
        /// <summary>
        /// Intializes a new instance of the <see cref="MoveVillageObjectData"/> class.
        /// </summary>
        public MoveVillageObjectData()
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
        /// <see cref="VillageObject"/> game index in <see cref="Village"/> that
        /// was moved.
        /// </summary>
        public int VillageObjectGameIndex;
    }
}
