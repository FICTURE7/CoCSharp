using CoCSharp.Csv;
using CoCSharp.Data;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans decoration (deco).
    /// </summary>
    public class Decoration : VillageObject
    {
        internal const int BaseGameID = 506000000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Decoration"/> class.
        /// </summary>
        public Decoration() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Decoration"/> class with the specified
        /// X coordinate and Y coordinate.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Decoration(int x, int y) : base(x, y)
        {
            // Space
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the <see cref="CsvData"/> expected
        /// by the <see cref="Decoration"/>.
        /// </summary>
        /// <remarks>
        /// This is needed to make sure that the user provides a proper <see cref="CsvData"/> type
        /// for the <see cref="VillageObject"/>.
        /// </remarks>
        protected override Type ExpectedDataType
        {
            get
            {
                return typeof(DecorationData);
            }
        }
    }
}
