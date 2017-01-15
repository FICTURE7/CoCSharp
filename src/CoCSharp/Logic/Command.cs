using CoCSharp.Network;
using CoCSharp.Network.Messages;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans command sent in <see cref="CommandMessage"/> and <see cref="AvailableServerCommandMessage"/>.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Maximum amount of embedded command allowed to be read.
        /// This field is constant.
        /// </summary>
        public const int MaxEmbeddedDepth = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        public Command()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="Command"/>.
        /// </summary>
        public abstract int Id { get; }

        /// <summary>
        /// Gets or sets the tick at which the command was executed.
        /// </summary>
        public int Tick { get; set; }

        /// <summary>
        /// Gets or sets the depth of the <see cref="Command"/>.
        /// </summary>
        protected internal int Depth { get; set; }

        /// <summary>
        /// Reads the <see cref="Command"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="Command"/>.
        /// </param>
        public abstract void ReadCommand(MessageReader reader);

        /// <summary>
        /// Writes the <see cref="Command"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="Command"/>.
        /// </param>
        public abstract void WriteCommand(MessageWriter writer);

        /// <summary>
        /// Performs the execution of the <see cref="Command"/> on the specified <see cref="Level"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> on which to perform the <see cref="Command"/>.</param>
        public virtual void Execute(Level level)
        {
            // Space
        }

        internal void ThrowIfReaderNull(MessageReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
        }

        internal void ThrowIfWriterNull(MessageWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
        }

        internal void ThrowIfLevelNull(Level level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));
        }

        internal void ThrowIfLevelVillageNull(Level level)
        {
            if (level.Village == null)
                throw new ArgumentNullException(nameof(level.Village));
        }
    }
}
