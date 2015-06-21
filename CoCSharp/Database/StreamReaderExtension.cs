using System.IO;

namespace CoCSharp.Database
{
    public static class StreamReaderExtension
    {
        public static void GotoLine(this StreamReader reader, int line)
        {
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < line; i++) reader.ReadLine();
        }
    }
}
