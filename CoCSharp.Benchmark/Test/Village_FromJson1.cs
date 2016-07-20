using CoCSharp.Logic;
using System.IO;

namespace CoCSharp.Benchmark.Test
{
    public class Village_FromJson1 : BenchmarkTest
    {
        public Village_FromJson1()
        {
            var path = "Content\\village.json";
            _villageJson = File.ReadAllText(path);
        }

        private readonly string _villageJson;
        private Village _village;

        public override void Execute()
        {
            _village = Village.FromJson(_villageJson);
        }

        public override void TearDown()
        {
            _village.Dispose();
        }
    }
}
