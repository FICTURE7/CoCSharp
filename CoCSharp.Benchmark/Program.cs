using CoCSharp.Benchmark.Test;
using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using System;
using System.Threading;

namespace CoCSharp.Benchmark
{
    public class Program
    {
        public static Benchmarker Benchmarker { get; set; }

        public static void Main(string[] args)
        {
            AssetManager.DefaultInstance = new AssetManager("Content");
            AssetManager.DefaultInstance.LoadCsv<BuildingData>("buildings.csv");
            AssetManager.DefaultInstance.LoadCsv<ObstacleData>("obstacles.csv");
            AssetManager.DefaultInstance.LoadCsv<TrapData>("traps.csv");
            AssetManager.DefaultInstance.LoadCsv<DecorationData>("decos.csv");

            Benchmarker = new Benchmarker();
#if (!DEBUG)
            Console.WriteLine("Press enter to being...");
            Console.ReadLine();
#endif
            //NOTE: Benchmarks were run on DEBUG mode.

            // ~0.00015ms
            //Run<CsvData_GetInstance>();

            // ~0.30ms with initial village.
            //Run<Village_FromJson1>();

            // ~2.17ms with maxed out village.
            //Run<Village_FromJson2>();

            // ~0.058ms with 30 Buildings objects.
            // ~0.18ms with 100 Building objects.
            // ~0.47ms with 100 Buildings, 100 Obstacles, 100 Traps and 100 Decorations.
            //Run<Village_ToJson1>();

            // ~2.0ms with a maxed out village excluding obstacles(cleared village).
            //Run<Village_ToJson2>();

            // ~0.00051ms with dictionary(Type,LogicComponent) implementation.
            // ~0.00021ms with array implementation.
            // ~0.00050ms with dictionary(Type,int) with array implementation.
            Run<VillageObject_AddComponent>();
            
            Console.WriteLine();
            Console.WriteLine("Benchmark done!");
            Console.ReadLine();
        }

        public static void Run<T>() where T : BenchmarkTest, new()
        {
            var result = Benchmarker.Run<T>();
            Console.WriteLine("---");
            Thread.Sleep(100);
        }
    }
}
