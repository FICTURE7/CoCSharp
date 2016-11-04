using NUnit.Framework;

namespace CoCSharp.Test
{
    [TestFixture]
    public class TokenUtilsTests
    {
        [Test]
        public void TestTokenUtilsCheckToken()
        {
            // Invalid token length.
            var token1 = "someinvalidtokenlength";
            Assert.False(TokenUtils.CheckToken(token1));

            // Invalid character in token.
            var token3 = "a4knrehaamcwt6cwafenamtaxx2nye2z8pmdt9c@";
            Assert.False(TokenUtils.CheckToken(token3));

            // Valid token.
            var token2 = "a4knrehaamcwt6cwafenamtaxx2nye2z8pmdt9cx";
            Assert.True(TokenUtils.CheckToken(token2));
        }

        [Test]
        public void TestTokenUtilsGenerateToken()
        {
            var token = TokenUtils.GenerateToken();
            Assert.True(TokenUtils.CheckToken(token));
        }
    }
}
