using System;
using System.IO;

namespace CoCSharp.Test
{
    public static class TestUtils
    {
        private static readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private static readonly string _contentDirectory = Path.Combine(_baseDirectory, "Content");
        public static string ContentDirectory { get { return _contentDirectory; } }

        private static readonly string _csvDirectory = Path.Combine(_contentDirectory, "csv");
        public static string CsvDirectory { get { return _csvDirectory; } }

        private static readonly string _layoutDirectory = Path.Combine(_contentDirectory, "layouts");
        public static string LayoutDirectory {  get { return _layoutDirectory; } }
    }
}
