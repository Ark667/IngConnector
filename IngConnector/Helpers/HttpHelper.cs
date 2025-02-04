using IngConnector.Extensions;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace IngConnector.Helpers;

public static class HttpHelper
{
    public static string GetHttpResponseContent(
        HttpClient client,
        HttpRequestMessage request,
        ILogger logger
    )
    {
        logger.LogInformation(request.ToRawString().Result);
        var response = client.Send(request);
        logger.LogInformation(response.ToRawString().Result);

        return response.Content.ReadAsStringAsync().Result;
    }

    public static HttpRequestMessage GetHttpRequest(
        string httpHost,
        HttpMethod httpMethod,
        string requestPath,
        string digest,
        string requestDate,
        string payload
    )
    {
        var request = new HttpRequestMessage(httpMethod, $"{httpHost}{requestPath}");
        request.Headers.Clear();
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Digest", digest);
        request.Headers.Add("Date", requestDate);
        request.Content = new StringContent(payload);
        return request;
    }

    public static HttpClient GetHttpClient(string certificatePath)
    {
        byte[] certBuffer = CertificateHelper.GetBytesFromPEM(
            File.ReadAllText(Path.Combine(certificatePath, $"example_client_tls.cer")),
            PemStringType.Certificate
        );
        byte[] keyBuffer = CertificateHelper.GetBytesFromPEM(
            File.ReadAllText(Path.Combine(certificatePath, $"example_client_tls.key")),
            PemStringType.RsaPrivateKey
        );

        X509Certificate2 certificate;
        using (var rsa = RSA.Create())
        {
            var publicCertificate = new X509Certificate2(certBuffer);
            rsa.ImportRSAPrivateKey(keyBuffer, out _);
            publicCertificate = publicCertificate.CopyWithPrivateKey(rsa);
            certificate = new X509Certificate2(publicCertificate.Export(X509ContentType.Pkcs12));
        }
        ;

        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(certificate);
        return new HttpClient(handler);
    }
}
