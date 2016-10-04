namespace CoCSharp.Csv
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TCsvData"></typeparam>
    public class CsvDataColumn<TCsvData> : CsvDataColumn where TCsvData : CsvData, new()
    {

    }
}