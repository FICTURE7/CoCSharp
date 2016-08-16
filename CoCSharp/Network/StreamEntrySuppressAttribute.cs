using System;

namespace CoCSharp.Network
{
    /// <summary>
    /// Use this attribute to prevent the <see cref="StreamEntryFactory"/> to add it
    /// in the <see cref="StreamEntryFactory.s_allianceStreamDictionary"/> 
    /// or <see cref="StreamEntryFactory.s_avatarStreamDictionary"/>. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal sealed class StreamEntrySuppressAttribute : Attribute
    {
        public StreamEntrySuppressAttribute(bool suppress = true)
        {
            Suppress = suppress;
        }

        public bool Suppress { get; private set; }
    }
}
