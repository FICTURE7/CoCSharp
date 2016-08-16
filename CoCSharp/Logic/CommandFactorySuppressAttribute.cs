using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Use this attribute to prevent the <see cref="CommandFactory"/> to add it
    /// in the <see cref="CommandFactory.s_commandDictionary"/>. This class cannot be inherited.
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
