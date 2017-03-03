using System.Security.Cryptography;

namespace CoCSharp.Network.Cryptography.NaCl.Internal
{
    internal static class curve25519xsalsa20poly1305
    {
        private static readonly RNGCryptoServiceProvider s_rng = new RNGCryptoServiceProvider();

        public const int crypto_secretbox_PUBLICKEYBYTES = 32;
        public const int crypto_secretbox_SECRETKEYBYTES = 32;
        public const int crypto_secretbox_BEFORENMBYTES = 32;
        public const int crypto_secretbox_NONCEBYTES = 24;
        public const int crypto_secretbox_ZEROBYTES = 32;
        public const int crypto_secretbox_BOXZEROBYTES = 16;

        public static int crypto_box_getpublickey(byte[] pk, byte[] sk)
        {
            return curve25519.crypto_scalarmult_base(pk, sk);
        }

        public static int crypto_box_keypair(byte[] pk, byte[] sk)
        {
            s_rng.GetBytes(sk);
            return curve25519.crypto_scalarmult_base(pk, sk);
        }

        public static int crypto_box_afternm(byte[] c, byte[] m, long mlen, byte[] n, byte[] k)
        {
            return xsalsa20poly1305.crypto_secretbox(c, m, mlen, n, k);
        }

        public static int crypto_box_beforenm(byte[] k, byte[] pk, byte[] sk)
        {
            byte[] sp = new byte[32], sigmap = xsalsa20.sigma;

            curve25519.crypto_scalarmult(sp, sk, pk);
            return hsalsa20.crypto_core(k, null, sp, sigmap);
        }

        public static int crypto_box(byte[] c, byte[] m, long mlen, byte[] n, byte[] pk, byte[] sk)
        {
            byte[] kp = new byte[crypto_secretbox_BEFORENMBYTES];

            crypto_box_beforenm(kp, pk, sk);
            return crypto_box_afternm(c, m, mlen, n, kp);
        }

        public static int crypto_box_open(byte[] m, byte[] c, long clen, byte[] n, byte[] pk, byte[] sk)
        {
            byte[] kp = new byte[crypto_secretbox_BEFORENMBYTES];

            crypto_box_beforenm(kp, pk, sk);
            return crypto_box_open_afternm(m, c, clen, n, kp);
        }

        public static int crypto_box_open_afternm(byte[] m, byte[] c, long clen, byte[] n, byte[] k)
        {
            return xsalsa20poly1305.crypto_secretbox_open(m, c, clen, n, k);
        }

        public static int crypto_box_afternm(byte[] c, byte[] m, byte[] n, byte[] k)
        {
            return crypto_box_afternm(c, m, m.Length, n, k);
        }

        public static int crypto_box_open_afternm(byte[] m, byte[] c, byte[] n, byte[] k)
        {
            return crypto_box_open_afternm(m, c, c.Length, n, k);
        }

        public static int crypto_box(byte[] c, byte[] m, byte[] n, byte[] pk, byte[] sk)
        {
            return crypto_box(c, m, m.Length, n, pk, sk);
        }

        public static int crypto_box_open(byte[] m, byte[] c, byte[] n, byte[] pk, byte[] sk)
        {
            return crypto_box_open(m, c, c.Length, n, pk, sk);
        }
    }
}
