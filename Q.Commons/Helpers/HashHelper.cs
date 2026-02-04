using System.Security.Cryptography;
using System.Text;

namespace Q.Commons.Helpers
{
    public static class HashHelper
    {
        private static string ToHashString(byte[] bytes)
        {
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public static string ComputeSha256Hash(Stream stream)
        {
            byte[] bytes = SHA256.HashData(stream);
            return ToHashString(bytes);
        }

        public static string ComputeSha256Hash(string input)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return ToHashString(bytes);
        }

        public static string ComputeMd5Hash(Stream input)
        {
            byte[] bytes = MD5.HashData(input);
            return ToHashString(bytes);
        }

        public static string ComputeMd5Hash(string input)
        {
            byte[] bytes = MD5.HashData(Encoding.UTF8.GetBytes(input));
            return ToHashString(bytes);
        }
    }
}
