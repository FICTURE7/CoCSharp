using CoCSharp.Csv;

namespace CoCSharp.Test.Csv
{
    // Test CsvData.
    public class TestData : CsvData
    {        
        internal override int KindID
        {
            get
            {
                return 420;
            }
        }
    }
}
