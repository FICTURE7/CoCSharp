using CoCSharp.Server.Api.Logging;
using System;

namespace CoCSharp.Server.Logging
{
    public class CleanErrorLogger : Logger
    {
        public override string Name => "error-clean";

        protected override string Write(object toLog)
        {
            return $"[{DateTime.UtcNow.ToString("yy/MM/dd - hh:mm:ss.fff")}][error-clean] - {toLog}";
        }
    }
}
