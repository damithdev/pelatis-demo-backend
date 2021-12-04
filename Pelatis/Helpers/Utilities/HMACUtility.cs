using System;
using System.Security.Cryptography;
using System.Text;

namespace Pelatis.Helpers.Utilities
{
    public class HMACUtility
    {
        public byte[] ComputeHash(ref byte[] salt, String text)
        {
            using var hmac = new HMACSHA512();
            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
            salt = hmac.Key;
            return computedHash;
        }


        public byte[] ComputeHashWithSalt(byte[] salt, String text)
        {
            using var hmac = new HMACSHA512(salt);
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(text));
        }
    }
}
