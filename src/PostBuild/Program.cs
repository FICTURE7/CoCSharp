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
            // args[0] = dst folder

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("PostBuild running...");

            if (!Directory.Exists(PackagesPath))
            {
                Console.WriteLine("Could not find /packages directory. Make sure to restore the nuget packages.");
                Environment.Exit(1);
            }

            var libsodiumVersion = string.Empty;
            var packageConfig = Path.Combine(CoCSharpPath, "packages.config");

            if (!File.Exists(packageConfig))
            {
                Console.WriteLine("Could not find /CoCSharp/packages.config.");
                Environment.Exit(1);
            }

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
            var outputPath = Path.Combine(PackagesPath, "libsodium-net." + libsodiumVersion, "output");

            if (!Directory.Exists(outputPath))
            {
                Console.WriteLine("Could not find packages/libsodium-net.ver/output directory.");
                Environment.Exit(1);
            }

            var dstPath = args.Length > 0 ? Path.Combine(CoCSharpPath, args[0]) : Path.Combine(CoCSharpPath, "bin/Debug");

            var dllSrc = Path.Combine(outputPath, "libsodium.dll");
            var dllDst = Path.Combine(dstPath, "libsodium.dll");
            if (!File.Exists(dllSrc))
            {
                Console.WriteLine("Could not find packages/libsodium-net.{0}/output/libsodium.dll file.", libsodiumVersion);
                Environment.Exit(1);
            }

            File.Copy(dllSrc, dllDst, true);

            var dll64Src = Path.Combine(outputPath, "libsodium-64.dll");
            var dll64Dst = Path.Combine(dstPath, "libsodium-64.dll");
            if (!File.Exists(dll64Src))
            {
                Console.WriteLine("Could not find packages/libsodium-net.{0}/output/libsodium-64.dll file.", libsodiumVersion);
                Environment.Exit(1);
            }

            File.Copy(dll64Src, dll64Dst, true);

            stopwatch.Stop();

            Console.WriteLine("Done in {0}ms", stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
