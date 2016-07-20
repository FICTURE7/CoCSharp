namespace CoCSharp.Benchmark
{
    public abstract class BenchmarkTest
    {
        public const int DefaultCount = 100000;

        // Name of the test.
        public virtual string Name { get; }

        // Amount of time to run the test.
        public virtual int Count
        {
            get
            {
                return DefaultCount;
            }
        }

        // Code to run before a test is run completely.
        public virtual void OneTimeSetUp()
        {
            // Space
        }

        // Code to run each time before a test is run.
        public virtual void SetUp()
        {
            // Space
        }

        // Code to run after a test is run completely.
        public virtual void OneTimeTearDown()
        {
            // Space
        }

        // Code to run each time after a test is run.
        public virtual void TearDown()
        {
            // Space
        }

        // Code to run.
        public abstract void Execute();
    }
}
