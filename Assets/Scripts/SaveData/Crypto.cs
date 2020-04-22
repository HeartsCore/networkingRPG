using System;


public static class Crypto
{
    #region Methods
    public static string CryptoXOR(string text, int key = 42)
    {
        var result = String.Empty;
        foreach (var simbol in text)
        {
            //I shift each character to key (42) - we get a different character
            result += (char)(simbol ^ key);
        }
        return result;
    }
    #endregion
}
