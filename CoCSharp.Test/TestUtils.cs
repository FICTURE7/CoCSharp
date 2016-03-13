using System;
using System.IO;

namespace CoCSharp.Test
{
    public static class TestUtils
    {
        private static readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private static readonly string _contentDirectory = Path.Combine(_baseDirectory, "Content");
        public static string ContentDirectory { get { return _contentDirectory; } }

        private static readonly string _csvDirectory = Path.Combine(_baseDirectory, "csv");
        public static string CsvDirecotry { get { return _csvDirectory; } }
    }
}
