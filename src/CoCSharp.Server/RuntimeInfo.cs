using System;

namespace CoCSharp.Server
{
    public static class RuntimeInfo
    {
        // Figure out if we're running on mono.
        public static bool IsMono => Type.GetType("Mono.Runtime") != null;
    }
}
