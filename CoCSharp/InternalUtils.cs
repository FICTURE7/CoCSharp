using CoCSharp.Network;
using System;
using System.IO;
using System.Net.Sockets;

namespace CoCSharp
{
    internal static class InternalUtils
    {
        public static Random Random = new Random();

        public static void DumpBuffer(SocketAsyncEventArgs args)
        {
            File.WriteAllBytes("dump", args.Buffer);
        }

        public static void DumpBuffer(MessageWriter writer)
        {
            File.WriteAllBytes("dump", ((MemoryStream)writer.BaseStream).ToArray());
        }

        public static void DumpBuffer(MessageReader reader)
        {
            File.WriteAllBytes("dump", ((MemoryStream)reader.BaseStream).ToArray());
        }

        public static bool CompareByteArray(byte[] a, byte[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }

        private const string ValidHexCharacters = "0123456789abcdef";

        public static bool IsValidHexString(string value)
        {
            if (value.Length == 0)
                return true;

            for (int i = 0; i < value.Length; i++)
            {
                if (!ValidHexCharacters.Contains(value[i].ToString()))
                    return false;
            }
            return true;
        }

        public static string BytesToString(byte[] bytes)
        {
            var str = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
                str += bytes[i].ToString("x2");
            return str;
        }
    }
}
