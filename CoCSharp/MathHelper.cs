using System;

namespace CoCSharp
{
    internal static class MathHelper
    {
        private static Random m_Random = new Random();
        public static Random Random { get { return m_Random; } }
    }
}
