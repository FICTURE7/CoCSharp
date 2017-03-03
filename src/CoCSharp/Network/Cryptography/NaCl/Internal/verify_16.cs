namespace CoCSharp.Network.Cryptography.NaCl.Internal
{
    internal static class verify_16
    {
        public static int crypto_verify(byte[] x, int xoffset, byte[] y)
        {
            int differentbits = 0;

            for (int i = 0; i < 15; i++)
                differentbits |= (x[xoffset + i] ^ y[i]) & 0xff;

            return (1 & (int)(((uint)differentbits - 1) >> 8)) - 1;
        }
    }
}
