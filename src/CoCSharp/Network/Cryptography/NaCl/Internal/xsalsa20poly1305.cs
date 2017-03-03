namespace CoCSharp.Network.Cryptography.NaCl.Internal
{
    internal static class xsalsa20poly1305
    {
        public const int crypto_secretbox_KEYBYTES = 32;
        public const int crypto_secretbox_NONCEBYTES = 24;
        public const int crypto_secretbox_ZEROBYTES = 32;
        public const int crypto_secretbox_BOXZEROBYTES = 16;

        public static int crypto_secretbox(byte[] c, byte[] m, long mlen, byte[] n, byte[] k)
        {
            if (mlen < 32)
                return -1;

            xsalsa20.crypto_stream_xor(c, m, mlen, n, k);
            poly1305.crypto_onetimeauth(c, 16, c, 32, mlen - 32, c);

            for (int i = 0; i < 16; ++i)
                c[i] = 0;

            return 0;
        }

        public static int crypto_secretbox_open(byte[] m, byte[] c, long clen, byte[] n, byte[] k)
        {
            if (clen < 32)
                return -1;

            byte[] subkeyp = new byte[32];

            xsalsa20.crypto_stream(subkeyp, 32, n, k);

            if (poly1305.crypto_onetimeauth_verify(c, 16, c, 32, clen - 32, subkeyp) != 0)
                return -1;

            xsalsa20.crypto_stream_xor(m, c, clen, n, k);

            for (int i = 0; i < 32; ++i)
                m[i] = 0;

            return 0;
        }
    }
}
