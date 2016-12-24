namespace CoCSharp
{
    /// <summary>
    /// Provides methods to manipulate user tokens.
    /// </summary>
    public static class TokenUtils
    {
        private const string TokenCharacters = "abcdefghijklmnopqrstuvwxyz1234567890";
        private const int TokenLength = 40;

        /// <summary>
        /// Determines if the specified string token is valid.
        /// </summary>
        /// <param name="token">Token to check.</param>
        /// <returns>Returns <c>true</c> if <paramref name="token"/> is valid.</returns>
        public static bool CheckToken(string token)
        {
            if (token == null)
                return false;
            if (token.Length != 40)
                return false;

            for (int i = 0; i < TokenLength; i++)
            {
                if (!TokenCharacters.Contains(token[i].ToString()))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Generates a valid user token.
        /// </summary>
        /// <returns>A valid user token.</returns>
        public static string GenerateToken()
        {
            var token = string.Empty;
            for (int i = 0; i < TokenLength; i++)
                token += TokenCharacters[InternalUtils.Random.Next(TokenCharacters.Length - 1)];
            return token;
        }

        // Thanks to kevintjuh93! at http://forum.ragezone.com/f916/id-hashtag-converter-method-1077827/
        // Just a slight modification from the original implementation. ^^
        private static readonly string TagChars = "0289PYLQGRJCUV";
        private static readonly int NumTagChars = TagChars.Length;
        public static string GetHashtagFromId(long id)
        {
            // Hashtag that we're going to return.
            var hashtag = string.Empty;

            var highInt = GetHighInt(id);
            if (highInt <= 255)
            {
                var lowInt = GetLowInt(id);

                id = (lowInt << 8) + highInt;
                while (id != 0)
                {
                    var index = id % NumTagChars;
                    hashtag = TagChars[(int)index] + hashtag;

                    id /= NumTagChars;
                }

                // Don't forget the hashtag at the end
                hashtag = "#" + hashtag;
            }

            return hashtag;
        }

        // Last 32 bits of the specified long.
        private static long GetLowInt(long l) => l & 0xFFFFFFFF;

        // First 32 bits of the specified long.
        private static long GetHighInt(long l) => l >> 32;
    }
}
