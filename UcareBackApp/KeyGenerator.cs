using System.Security.Cryptography;
using System;

public static class KeyGenerator
{
    public static string GenerateSecureKey()
    {
        byte[] keyBytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(keyBytes);
    }}
