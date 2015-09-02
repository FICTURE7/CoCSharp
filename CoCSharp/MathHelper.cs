using System;

namespace CoCSharp
{
    internal static class MathHelper
    {
        private static Random _Random = new Random();
        public static Random Random { get { return _Random; } }
    }
}
