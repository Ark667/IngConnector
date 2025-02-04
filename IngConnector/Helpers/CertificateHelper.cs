using System;

namespace IngConnector.Helpers;

public static class CertificateHelper
{
    public static byte[] GetSHA256(byte[] phrase)
    {
        var crypt = System.Security.Cryptography.SHA256.Create();
        return crypt.ComputeHash(phrase);
    }

    public static byte[] GetBytesFromPEM(string pemString, PemStringType type)
    {
        string header;
        string footer;
        switch (type)
        {
            case PemStringType.Certificate:
                header = "-----BEGIN CERTIFICATE-----";
                footer = "-----END CERTIFICATE-----";
                break;
            case PemStringType.RsaPrivateKey:
                header = "-----BEGIN RSA PRIVATE KEY-----";
                footer = "-----END RSA PRIVATE KEY-----";
                break;
            default:
                return null;
        }

        int start = pemString.IndexOf(header) + header.Length;
        int end = pemString.IndexOf(footer, start) - start;
        return Convert.FromBase64String(pemString.Substring(start, end));
    }
}
