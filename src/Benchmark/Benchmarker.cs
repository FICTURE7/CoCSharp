using System;
using System.Diagnostics;

namespace CoCSharp.Benchmark
{
    public class Benchmarker
    {
        public Benchmarker()
        {
            _sw = new Stopwatch();
        }

        private readonly Stopwatch _sw;

        public BenchmarkResult Run<T>() where T : BenchmarkTest, new()
        {
            var instance = new T();
            var type = typeof(T);

            var name = instance.Name == null ? type.Name.Replace("_", ".") : instance.Name;
            var count = instance.Count;
            var shouldOneTimeSetUp = type.GetMethod("OneTimeSetUp").DeclaringType == type;
            var shouldSetUp = type.GetMethod("SetUp").DeclaringType == type;
            var shouldOneTimeTearDown = type.GetMethod("OneTimeTearDown").DeclaringType == type;
            var shouldTearDown = type.GetMethod("TearDown").DeclaringType == type;

            Console.WriteLine(name + ":" + count);

            if (shouldOneTimeSetUp)
                instance.OneTimeSetUp();

            for (int i = 0; i < count; i++)
            {
                if (shouldSetUp)
                    instance.SetUp();

                _sw.Start();
                instance.Execute();
                _sw.Stop();

                if (shouldTearDown)
                    instance.TearDown();
            }

            if (shouldOneTimeTearDown)
                instance.OneTimeTearDown();

            var result = new BenchmarkResult(name, count, _sw.Elapsed.TotalMilliseconds);
            Console.WriteLine("\t-> Duration {0}ms\r\n\t-> Average {1}ms", result.Duration, result.Average);
            return result;
        }
    }
}
