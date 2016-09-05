using CoCSharp.Csv;
using CoCSharp.Data.Models;
using System;

namespace CoCSharp.Benchmark.Test
{
    public class CsvConvert_Deserialize : BenchmarkTest
    {
        public CsvConvert_Deserialize()
        {
            _table = new CsvTable("Content/buildings.csv");
        }

        public override int Count
        {
            get
            {
                return 5000;
            }
        }

        private readonly CsvTable _table;

        public override void Execute()
        {
            CsvConvert.Deserialize(_table, typeof(BuildingData));
        }
    }
}
