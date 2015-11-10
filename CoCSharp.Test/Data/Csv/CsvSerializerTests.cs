using CoCSharp.Data;
using CoCSharp.Data.Csv;
using NUnit.Framework;
using System;

namespace CoCSharp.Test.Csv.Data
{
    [TestFixture]
    public class CsvSerializerTests
    {
        //private CsvTable _Table = (CsvTable)null;

        [SetUp]
        public void Initialize()
        {
            // _Table = new CsvTable("Resources//characters.csv");
        }

        [Test]
        public void TestDeserialize()
        {
            //var objs = CsvSerializer.Deserialize(_Table, typeof(CharacterData));
            //foreach (var obj in objs)
            //{
            //    var data = (CoCData)obj;
            //    Console.WriteLine(data.ID);
            //}
            Console.WriteLine("Running empty test!");
        }
    }
}
