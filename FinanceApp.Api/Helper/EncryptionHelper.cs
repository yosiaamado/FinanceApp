namespace FinanceApp.Api.Helper
{
    public static class EncryptionHelper
    {
        public static string Hash(string text)
        {
            string hashed = BCrypt.Net.BCrypt.HashPassword(text);
            return hashed;
        }

        public static bool Verify(string text, string hashed)
        {
            bool isMatch = BCrypt.Net.BCrypt.EnhancedVerify(text, hashed);
            return isMatch;
        }
    }
}
