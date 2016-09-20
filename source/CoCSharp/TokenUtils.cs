namespace CoCSharp
{
    /// <summary>
    /// Provides methods to manipulate user tokens.
    /// </summary>
    public static class TokenUtils
    {
        //TODO: Implement AuthCredentials class.

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
    }
}
