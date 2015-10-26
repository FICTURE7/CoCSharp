using SevenZip.Compression.LZMA;
using System;
using System.IO;

namespace SevenZip
{
    //Peter Bromberg's helper code. Big thanks for that.

    /// <summary>
    /// Provides methods to compress or decompress LZMA <see cref="byte"/> array.
    /// </summary>
    public static class LzmaUtils
    {
        private static int Dictionary = 1 << 23;
        private static bool EndOfStream = false;

        private static CoderPropID[] PropertiesIDs =
        {
            CoderPropID.DictionarySize,
            CoderPropID.PosStateBits,
            CoderPropID.LitContextBits,
            CoderPropID.LitPosBits,
            CoderPropID.Algorithm,
            CoderPropID.NumFastBytes,
            CoderPropID.MatchFinder,
            CoderPropID.EndMarker,
        };

        private static object[] Properties =
        {
            Dictionary,
            2,
            3,
            0,
            2,
            64,
            "bt4",
            EndOfStream
        };

        /// <summary>
        /// Compresses the specified <see cref="byte"/> array into
        /// LZMA.
        /// </summary>
        /// <param name="bytes">The <see cref="byte"/> array to compress.</param>
        /// <returns>The compressed <see cref="byte"/> array.</returns>
        public static byte[] Compress(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            using (var inStream = new MemoryStream(bytes))
            {
                using (var outStream = new MemoryStream())
                {
                    var encoder = new Encoder();

                    encoder.SetCoderProperties(PropertiesIDs, Properties);
                    encoder.WriteCoderProperties(outStream);
                    var fileSize = inStream.Length;
                    for (int i = 0; i < 8; i++)
                        outStream.WriteByte((byte)(fileSize >> (8 * i)));

                    encoder.Code(inStream, outStream, -1, -1, null);
                    return outStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Decompresses the specified <see cref="byte"/> into
        /// LZMA.
        /// </summary>
        /// <param name="bytes">The <see cref="byte"/> array to decompress.</param>
        /// <returns>The decompressed <see cref="byte"/> array.</returns>
        public static byte[] Decompress(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            using (var outStream = new MemoryStream())
            {
                using (var inStream = new MemoryStream(bytes))
                {
                    var decoder = new Decoder();

                    inStream.Seek(0, 0);
                    var properties = new byte[5];
                    if (inStream.Read(properties, 0, 5) != 5)
                        throw new Exception("Input .lzma is too short");

                    var outSize = 0L;
                    for (int i = 0; i < 8; i++)
                    {
                        var v = inStream.ReadByte();
                        if (v < 0)
                            throw new Exception("Can't Read 1");

                        outSize |= ((long)v) << (8 * i);
                    }

                    var compressedSize = inStream.Length - inStream.Position;
                    decoder.SetDecoderProperties(properties);
                    decoder.Code(inStream, outStream, compressedSize, outSize, null);
                    return outStream.ToArray();
                }
            }
        }
    }
}
