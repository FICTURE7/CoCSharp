using SevenZip.Compression.LZMA;
using System;
using System.IO;

namespace SevenZip
{
    //Peter Bromberg's helper code. Big thanks for that.

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

        // these are the default properties, keeping it simple for now:
        private static object[] Properties = 
        {
			(Int32)(Dictionary),
			(Int32)(2),
			(Int32)(3),
			(Int32)(0),
			(Int32)(2),
			(Int32)(64),
			"bt4",
			EndOfStream
        };

        /// <summary>
        /// Compresses the inputBytes in Lzma.
        /// </summary>
        /// <param name="inputBytes">Bytes to compress.</param>
        /// <returns>Compressed bytes.</returns>
        public static byte[] Compress(byte[] inputBytes)
        {
            if (inputBytes == null) 
                throw new ArgumentNullException("inputBytes");

            MemoryStream inStream = new MemoryStream(inputBytes);
            MemoryStream outStream = new MemoryStream();
            Encoder encoder = new Encoder();

            encoder.SetCoderProperties(PropertiesIDs, Properties);
            encoder.WriteCoderProperties(outStream);
            var fileSize = inStream.Length;
            for (int i = 0; i < 8; i++)
                outStream.WriteByte((Byte)(fileSize >> (8 * i)));

            encoder.Code(inStream, outStream, -1, -1, null);
            return outStream.ToArray();
        }

        /// <summary>
        /// Decompresses the inputBytes from Lzma
        /// </summary>
        /// <param name="inputBytes">bytes to decompress</param>
        /// <returns>Decompressed bytes</returns>
        public static byte[] Decompress(byte[] inputBytes)
        {
            if (inputBytes == null) 
                throw new ArgumentNullException("inputBytes");

            var decoder = new Decoder();
            var newOutStream = new MemoryStream();
            var newInStream = new MemoryStream(inputBytes);

            newInStream.Seek(0, 0);
            var properties2 = new byte[5];
            if (newInStream.Read(properties2, 0, 5) != 5)
                throw (new Exception("input .lzma is too short"));

            var outSize = 0L;
            for (int i = 0; i < 8; i++)
            {
                var v = newInStream.ReadByte();
                if (v < 0)
                    throw (new Exception("Can't Read 1"));

                outSize |= ((long)(byte)v) << (8 * i);
            }

            decoder.SetDecoderProperties(properties2);
            var compressedSize = newInStream.Length - newInStream.Position;
            decoder.Code(newInStream, newOutStream, compressedSize, outSize, null);
            return newOutStream.ToArray();
        }
    }
}
