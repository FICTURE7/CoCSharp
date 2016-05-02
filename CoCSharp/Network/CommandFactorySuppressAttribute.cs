using System;

namespace CoCSharp.Network
{
    /// <summary>
    /// Use this attribute to prevent the <see cref="CommandFactory"/> to add it
    /// in the <see cref="CommandFactory.CommandDictionary"/>. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal sealed class CommandFactorySuppressAttribute : Attribute
    {
        public CommandFactorySuppressAttribute(bool suppress = true)
        {
            Suppress = suppress;
        }

        public bool Suppress { get; private set; }
    }
}
