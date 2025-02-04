using IngConnector.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace IngConnector.Services;

public class IngService
{
    public ILogger<IngService> Logger { get; }

    public string CertificatesPath { get; }
    public string ServiceHost { get; }
    public string AccessTokenKeyId { get; }
    public string GetKeyId { get; }

    public IngService(IConfiguration configuration, ILogger<IngService> logger)
    {
        CertificatesPath = configuration["IngConnector:CertificatesPath"];
        ServiceHost = configuration["IngConnector:ServiceHost"];
        AccessTokenKeyId = configuration["IngConnector:KeyId:AccessToken"];
        GetKeyId = configuration["IngConnector:KeyId:Get"];
        Logger = logger;
    }

    public string GetAccessToken()
    {
        var httpMethod = "post";
        var reqPath = "/oauth2/token";
        var payload = "grant_type=client_credentials";

        var digest = GetDigest(payload);
        string reqDate = GetDate();
        string signingString = GetSigningString(httpMethod, reqPath, digest, reqDate);
        string signature = GetSignature(signingString, CertificatesPath);

        using (var client = HttpHelper.GetHttpClient(CertificatesPath))
        {
            var request = HttpHelper.GetHttpRequest(
                ServiceHost,
                HttpMethod.Post,
                reqPath,
                digest,
                reqDate,
                payload
            );
            request.Headers.Add(
                "authorization",
                $"Signature keyId=\"{AccessTokenKeyId}\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"{signature}\""
            );
            request.Headers.Add(
                "TPP-Signature-Certificate",
                "-----BEGIN CERTIFICATE-----MIIENjCCAx6gAwIBAgIEXkKZvjANBgkqhkiG9w0BAQsFADByMR8wHQYDVQQDDBZBcHBDZXJ0aWZpY2F0ZU1lYW5zQVBJMQwwCgYDVQQLDANJTkcxDDAKBgNVBAoMA0lORzESMBAGA1UEBwwJQW1zdGVyZGFtMRIwEAYDVQQIDAlBbXN0ZXJkYW0xCzAJBgNVBAYTAk5MMB4XDTIwMDIxMDEyMTAzOFoXDTIzMDIxMTEyMTAzOFowPjEdMBsGA1UECwwUc2FuZGJveF9laWRhc19xc2VhbGMxHTAbBgNVBGEMFFBTRE5MLVNCWC0xMjM0NTEyMzQ1MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkJltvbEo4/SFcvtGiRCar7Ah/aP0pY0bsAaCFwdgPikzFj+ij3TYgZLykz40EHODtG5Fz0iZD3fjBRRM/gsFPlUPSntgUEPiBG2VUMKbR6P/KQOzmNKF7zcOly0JVOyWcTTAi0VAl3MEO/nlSfrKVSROzdT4Aw/h2RVy5qlw66jmCTcp5H5kMiz6BGpG+K0dxqBTJP1WTYJhcEj6g0r0SYMnjKxBnztuhX5XylqoVdUy1a1ouMXU8IjWPDjEaM1TcPXczJFhakkAneoAyN6ztrII2xQ5mqmEQXV4BY/iQLT2grLYOvF2hlMg0kdtK3LXoPlbaAUmXCoO8VCfyWZvqwIDAQABo4IBBjCCAQIwNwYDVR0fBDAwLjAsoCqgKIYmaHR0cHM6Ly93d3cuaW5nLm5sL3Rlc3QvZWlkYXMvdGVzdC5jcmwwHwYDVR0jBBgwFoAUcEi7XgDA9Cb4xHTReNLETt+0clkwHQYDVR0OBBYEFLQI1Hig4yPUm6xIygThkbr60X8wMIGGBggrBgEFBQcBAwR6MHgwCgYGBACORgEBDAAwEwYGBACORgEGMAkGBwQAjkYBBgIwVQYGBACBmCcCMEswOTARBgcEAIGYJwEDDAZQU1BfQUkwEQYHBACBmCcBAQwGUFNQX0FTMBEGBwQAgZgnAQIMBlBTUF9QSQwGWC1XSU5HDAZOTC1YV0cwDQYJKoZIhvcNAQELBQADggEBAEW0Rq1KsLZooH27QfYQYy2MRpttoubtWFIyUV0Fc+RdIjtRyuS6Zx9j8kbEyEhXDi1CEVVeEfwDtwcw5Y3w6Prm9HULLh4yzgIKMcAsDB0ooNrmDwdsYcU/Oju23ym+6rWRcPkZE1on6QSkq8avBfrcxSBKrEbmodnJqUWeUv+oAKKG3W47U5hpcLSYKXVfBK1J2fnk1jxdE3mWeezoaTkGMQpBBARN0zMQGOTNPHKSsTYbLRCCGxcbf5oy8nHTfJpW4WO6rK8qcFTDOWzsW0sRxYviZFAJd8rRUCnxkZKQHIxeJXNQrrNrJrekLH3FbAm/LkyWk4Mw1w0TnQLAq+s=-----END CERTIFICATE-----"
            );
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(
                "application/x-www-form-urlencoded"
            );

            return JsonDocument
                .Parse(HttpHelper.GetHttpResponseContent(client, request, Logger))
                .RootElement.GetProperty("access_token")
                .ToString();
        }
    }

    public string GetBalance(string accessToken)
    {
        return Get(
            accessToken,
            "/v3/accounts/a217d676-7559-4f2a-83dc-5da0c2279223/balances?balanceTypes=interimBooked"
        );
    }

    public string GetTransactions(string accessToken)
    {
        return Get(accessToken, "/v3/accounts/a217d676-7559-4f2a-83dc-5da0c2279223/transactions");
    }

    private string Get(string accessToken, string requestPath)
    {
        var httpMethod = "get";

        var digest = GetDigest(string.Empty);
        var reqDate = GetDate();
        var signingString = GetSigningString(httpMethod, requestPath, digest, reqDate);
        var signature = GetSignature(signingString, CertificatesPath);

        using (var client = HttpHelper.GetHttpClient(CertificatesPath))
        {
            var request = HttpHelper.GetHttpRequest(
                ServiceHost,
                HttpMethod.Get,
                requestPath,
                digest,
                reqDate,
                string.Empty
            );
            request.Headers.Add("X-Request-ID", $"{Guid.NewGuid()}");
            request.Headers.Add("Authorization", $"Bearer {accessToken}");
            request.Headers.Add(
                "Signature",
                $"keyId=\"{GetKeyId}\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"{signature}\""
            );
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            return HttpHelper.GetHttpResponseContent(client, request, Logger);
        }
    }

    private static string GetSignature(string signingString, string certificatePath)
    {
        string signature;
        using (var rsa = RSA.Create())
        {
            rsa.ImportRSAPrivateKey(
                CertificateHelper.GetBytesFromPEM(
                    File.ReadAllText(Path.Combine(certificatePath, "example_client_signing.key")),
                    PemStringType.RsaPrivateKey
                ),
                out _
            );
            var data = Encoding.UTF8.GetBytes(signingString);
            signature = Convert.ToBase64String(
                rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1)
            );
        }

        return signature;
    }

    private static string GetSigningString(
        string httpMethod,
        string requestPath,
        string digest,
        string requestDate
    )
    {
        return $"(request-target): {httpMethod} {requestPath}{'\n'}date: {requestDate}{'\n'}digest: {digest}";
    }

    private static string GetDate()
    {
        return DateTime.UtcNow.ToString(
            "ddd',' dd MMM yyyy HH':'mm':'ss 'GMT'",
            System.Globalization.DateTimeFormatInfo.InvariantInfo
        );
    }

    private static string GetDigest(string payload)
    {
        var payloadDigest = Convert.ToBase64String(
            CertificateHelper.GetSHA256(Encoding.UTF8.GetBytes(payload))
        );
        var digest = $"SHA-256={payloadDigest}";
        return digest;
    }
}
