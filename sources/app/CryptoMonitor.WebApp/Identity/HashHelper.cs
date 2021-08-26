using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CryptoMonitor.WebApp.Identity
{
    internal static class HashHelper
    {
        private const string Salt = "CGYzqeN4plZekNC88Umm1Q==";

        internal static string Hash(string input)
        {
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: input,
                salt: Convert.FromBase64String(Salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}