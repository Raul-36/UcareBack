using System.Security.Cryptography;
using System;

public static class KeyGenerator
{
    public static string GenerateSecureKey()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            var keyBytes = new byte[32];
            rng.GetBytes(keyBytes);
            return Convert.ToBase64String(keyBytes);
        }
    }
}
