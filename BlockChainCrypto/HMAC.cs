using System.Security.Cryptography;

namespace BlockChainCrypto
{
    public class HMAC
    {
        private const int KeySize = 32;

        public static byte[] GenerateKey()
        {
            using (var randomMunberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[KeySize];
                randomMunberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        public static byte[] ComputeHmacSha256(byte[] toBeHashed, byte[] key)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(toBeHashed);
            }
        }
    }
}
