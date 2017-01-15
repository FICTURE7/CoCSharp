using CoCSharp.Server.Api.Logging;
using System;

namespace CoCSharp.Server.Logging
{
    public class ClanLogger : Logger
    {
        public override string Name => "clan";

        protected override string Write(object toLog)
        {
            return $"[{DateTime.UtcNow.ToString("yy/MM/dd - hh:mm:ss.fff")}][clan] - {toLog}";
        }
    }
}
