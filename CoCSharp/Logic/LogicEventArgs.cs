using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Base class for logical events.
    /// </summary>
    public class LogicEventArgs : EventArgs
    {
        internal LogicEventArgs(LogicOperation op)
        {
            Operation = op;
        }

        /// <summary>
        /// Gets the <see cref="LogicOperation"/> that was made.
        /// </summary>
        public LogicOperation Operation { get; private set; }

        internal static bool IsValidLogicOperation(LogicOperation op)
        {
            if (op.HasFlag(LogicOperation.Cancel) && op.HasFlag(LogicOperation.Finished))
                return false;

            if (op.HasFlag(LogicOperation.Started) && op.HasFlag(LogicOperation.Cancel) ||
                op.HasFlag(LogicOperation.Started) && op.HasFlag(LogicOperation.Finished))
                return false;

            return true;
        }
    }
}
