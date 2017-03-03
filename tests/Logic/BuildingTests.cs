using CoCSharp.Data;
using CoCSharp.Logic;
using NUnit.Framework;
using System.IO;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class BuildingTests
    {
        private Village _village;
        private readonly AssetManager _assets;
        public BuildingTests()
        {
            _assets = new AssetManager(TestUtils.CsvDirectory);
            AssetManager.Default = _assets;
        }

        [SetUp]
        public void SetUp()
        {
            _village = new Village();
        }

        public static string Load(string name)
        {
            return File.ReadAllText(Path.Combine(TestUtils.LayoutDirectory, name + ".json"));
        }
    }
}
