using System;

namespace CoCSharp.Network
{
    /// <summary>
    /// Use this attribute to prevent the <see cref="MessageFactory"/> to add it
    /// in the <see cref="MessageFactory.MessageDictionary"/>. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal sealed class MessageFactorySuppressAttribute : Attribute
    {
        public MessageFactorySuppressAttribute(bool suppress = true)
        {
            Suppress = suppress;
        }

        public bool Suppress { get; private set; }
    }
}
