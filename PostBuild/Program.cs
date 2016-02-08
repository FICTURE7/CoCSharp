using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace PostBuild
{
    public class Program
    {
        public static string PackagesPath { get { return "../../../packages/"; } }
        public static string CoCSharpPath { get { return "../../../CoCSharp/"; } }

        public static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("PostBuild running...");

            var libsodiumVersion = string.Empty;
            var packageConfig = Path.Combine(CoCSharpPath, "packages.config");

            Console.WriteLine("Reading libsodium-net version from {0}...", packageConfig);
            using (var reader = XmlReader.Create(packageConfig))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "package" && reader["id"] == "libsodium-net")
                        {
                            libsodiumVersion = reader["version"];
                            break;
                        }
                    }
                }
            }

            if (libsodiumVersion == string.Empty)
            {
                Console.WriteLine("Could not find libsodium-net version from packages.config.");
                Environment.Exit(1);
            }

            Console.WriteLine("Copying libsodium natives version {0}...", libsodiumVersion);
            var libsodiumPath = Path.Combine(PackagesPath, "libsodium-net." + libsodiumVersion, "output");
            var dstPath = Path.Combine(CoCSharpPath, "bin", "Debug");

            File.Copy(Path.Combine(libsodiumPath, "libsodium.dll"), Path.Combine(dstPath, "libsodium.dll"), true);
            File.Copy(Path.Combine(libsodiumPath, "libsodium-64.dll"), Path.Combine(dstPath, "libsodium-64.dll"), true);

            stopwatch.Stop();

            Console.WriteLine("Done in {0}ms", stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
