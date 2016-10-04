using CoCSharp.Csv;
using CoCSharp.Data.Models;

namespace CoCSharp.Benchmark.Test
{
    public class CsvData_GetInstance : BenchmarkTest
    {
        public CsvData_GetInstance()
        {
            var instance = CsvData.GetInstance<BuildingData>();
        }

        public override void Execute()
        {
            CsvData.GetInstance<BuildingData>();
        }
    }
}
