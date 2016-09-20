using CoCSharp.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CoCSharp.Benchmark.Test
{
    public class Village_FromJson2 : BenchmarkTest
    {
        public Village_FromJson2()
        {
            var path = "Content\\village_max.json";
            _villageJson = File.ReadAllText(path);
            _refs = new List<WeakReference>();
        }

        private readonly string _villageJson;

        //public override int Count
        //{
        //    get
        //    {
        //        return 500;
        //    }
        //}

        private List<WeakReference> _refs;
        private Village _village;


        public override void Execute()
        {
            _village = Village.FromJson(_villageJson);
        }

        public override void TearDown()
        {
            _refs.Add(new WeakReference(_village));
            _village.Dispose();
        }

        public override void OneTimeTearDown()
        {
            Console.WriteLine("Created: {0}", VillageObject.s_created);
            Console.WriteLine("Rested: {0}", VillageObject.s_rested);
            Console.WriteLine("InPool: {0}", VillageObjectPool.TotalCount);
            Console.WriteLine("Alive: {0}", _refs.Count(r => r.IsAlive));

            _village = null;
            Console.WriteLine("\r\nGC.Collect()\r\n");
            GC.Collect();

            Console.WriteLine("Created: {0}", VillageObject.s_created);
            Console.WriteLine("Rested: {0}", VillageObject.s_rested);
            Console.WriteLine("InPool: {0}", VillageObjectPool.TotalCount);
            Console.WriteLine("Alive: {0}", _refs.Count(r => r.IsAlive));
        }
    }
}
