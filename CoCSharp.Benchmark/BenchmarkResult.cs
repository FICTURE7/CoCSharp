namespace CoCSharp.Benchmark
{
    public class BenchmarkResult
    {
        public BenchmarkResult(string name, int count, double duration)
        {
            Name = name;
            Count = count;
            Duration = duration;
        }

        public string Name { get; set; }
        public int Count { get; set; }
        public double Duration { get; set; }
        public double Average
        {
            get
            {
                return Duration / Count;
            }
        }

        public override string ToString()
        {
            return string.Format("{{{0}}}:({{{1}ms}},{{{2}ms}})", Name, Duration, Average);
        }
    }
}
