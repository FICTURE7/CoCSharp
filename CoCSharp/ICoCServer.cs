using CoCSharp.Logging;
using System.Collections.Generic;

namespace CoCSharp
{
    public interface ICoCServer
    {
        List<ILogger> Loggers { get; }
    }
}
