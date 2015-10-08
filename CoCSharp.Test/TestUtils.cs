using System;

namespace CoCSharp.Test
{
    public static class TestUtils
    {
        public static void Print(byte[] array)
        {
            var str = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                str += array[i].ToString("X2") + " ";
                if (i % 32 == 31)
                {
                    Console.WriteLine(str);
                    str = string.Empty;
                }
            }
            if (array.Length < 32)
                Console.WriteLine(str);
        }
    }
}
