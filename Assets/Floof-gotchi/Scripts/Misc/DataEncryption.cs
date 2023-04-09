using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEncryption
{
    private static readonly string KEY = "464c4f4f46";
    public static string Encrypt(string plainText)
    {
        var cipherText = XORCipher(plainText, KEY);
        return cipherText;
    }
    public static string Decrypt(string cipherText)
    {
        var plainText = XORCipher(cipherText, KEY);
        return plainText;
    }
    
    private static string XORCipher(string data, string key)
    {
        var dataLength = data.Length;
        var keyLength = key.Length;
        var output = data.ToCharArray();

        for (int i = 0; i < dataLength; ++i)
        {
            output[i] = (char)(data[i] ^ key[i % keyLength]);
        }

        return new string(output);
    }
}
