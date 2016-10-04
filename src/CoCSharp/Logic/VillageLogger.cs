using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CoCSharp.Logic
{
    internal class VillageLogger
    {
        public VillageLogger()
        {
            _mainBuilder = new StringBuilder();
            _tmpBuilder = new StringBuilder();
            _ident = Guid.NewGuid();
        }

        private readonly Guid _ident;
        private readonly StringBuilder _mainBuilder;
        private readonly StringBuilder _tmpBuilder;

        [Conditional("DEBUG")]
        public void Info(int tick, string info)
        {
            var now = DateTime.Now.ToString("HH:mm:ss.fff");
            _tmpBuilder.AppendFormat("[{0}::{1}]: ", tick, now)
                       .AppendLine(info);
            _mainBuilder.Append(_tmpBuilder);
            Debug.Write(_tmpBuilder);
            _tmpBuilder.Clear();
        }

        [Conditional("DEBUG")]
        public void Info(int tick, string info, params object[] args)
        {
            Info(tick, string.Format(info, args));
        }

        [Conditional("DEBUG")]
        public void Dump()
        {
            var path = "village-" + _ident + ".log";
            var log = _mainBuilder.ToString();
            File.WriteAllText(path, log);
        }
    }
}
