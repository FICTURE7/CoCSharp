using CoCSharp.Csv;

namespace CoCSharp.Test.Csv
{
    // Test CsvData.
    public class TestData : CsvData
    {        
        internal override int KindId
        {
            get
            {
                return 420;
            }
        }
    }
}
