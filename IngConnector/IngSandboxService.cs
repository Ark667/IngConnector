using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace IngConnector
{
    public class IngSandboxService
    {
        public string CertificatesPath { get; }
        public string ServiceHost { get; }
        public string KeyId { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="IngSandboxService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        public IngSandboxService(IConfiguration configuration)
        {
            CertificatesPath = configuration["IngConnector:CertificatesPath"];
            ServiceHost = configuration["IngConnector:ServiceHost"];
            KeyId = configuration["IngConnector:KeyId"];
        }


        public string GetAccessToken()
        {
            // ## THE SCRIPT USES THE DOWNLOADED EXAMPLE EIDAS CERTIFICATES
            // keyId="SN=5E4299BE" # Serial number of the downlaoded certificate in hexadecimal code
            // certPath="./certs/" # path of the downloaded certificates and keys
            // httpHost="https://api.sandbox.ing.com"
            //var keyId = "SN=5E4299BE";

            // # httpMethod value must be in lower case
            // httpMethod="post"
            // reqPath="/oauth2/token"
            var httpMethod = "post";
            var reqPath = "/oauth2/token";

            // # You can also provide scope parameter in the body E.g. "grant_type=client_credentials&scope=greetings%3Aview"
            // # scope is an optional parameter. If you don't provide a scope, the accessToken is returned for all available scopes
            // payload="grant_type=client_credentials"
            // payloadDigest=`echo -n "$payload" | openssl dgst -binary -sha256 | openssl base64`
            // digest=SHA-256=$payloadDigest
            var payload = "grant_type=client_credentials";
            var digest = GetDigest(payload);
            string reqDate = GetDate();
            string signingString = GetSigningString(httpMethod, reqPath, digest, reqDate);

            // signature=`printf %s "$signingString" | openssl dgst -sha256 -sign "${certPath}example_client_signing.key" -passin "pass:changeit" | openssl base64 -A`
            string signature = GetSignature(signingString, CertificatesPath);

            // # Curl request method must be in uppercase e.g "POST", "GET"
            // curl -i -X POST "${httpHost}${reqPath}" \
            // -H 'Accept: application/json' \
            // -H 'Content-Type: application/x-www-form-urlencoded' \
            // -H "Digest: ${digest}" \
            // -H "Date: ${reqDate}" \
            // -H "TPP-Signature-Certificate: -----BEGIN CERTIFICATE-----MIIENjCCAx6gAwIBAgIEXkKZvjANBgkqhkiG9w0BAQsFADByMR8wHQYDVQQDDBZBcHBDZXJ0aWZpY2F0ZU1lYW5zQVBJMQwwCgYDVQQLDANJTkcxDDAKBgNVBAoMA0lORzESMBAGA1UEBwwJQW1zdGVyZGFtMRIwEAYDVQQIDAlBbXN0ZXJkYW0xCzAJBgNVBAYTAk5MMB4XDTIwMDIxMDEyMTAzOFoXDTIzMDIxMTEyMTAzOFowPjEdMBsGA1UECwwUc2FuZGJveF9laWRhc19xc2VhbGMxHTAbBgNVBGEMFFBTRE5MLVNCWC0xMjM0NTEyMzQ1MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkJltvbEo4/SFcvtGiRCar7Ah/aP0pY0bsAaCFwdgPikzFj+ij3TYgZLykz40EHODtG5Fz0iZD3fjBRRM/gsFPlUPSntgUEPiBG2VUMKbR6P/KQOzmNKF7zcOly0JVOyWcTTAi0VAl3MEO/nlSfrKVSROzdT4Aw/h2RVy5qlw66jmCTcp5H5kMiz6BGpG+K0dxqBTJP1WTYJhcEj6g0r0SYMnjKxBnztuhX5XylqoVdUy1a1ouMXU8IjWPDjEaM1TcPXczJFhakkAneoAyN6ztrII2xQ5mqmEQXV4BY/iQLT2grLYOvF2hlMg0kdtK3LXoPlbaAUmXCoO8VCfyWZvqwIDAQABo4IBBjCCAQIwNwYDVR0fBDAwLjAsoCqgKIYmaHR0cHM6Ly93d3cuaW5nLm5sL3Rlc3QvZWlkYXMvdGVzdC5jcmwwHwYDVR0jBBgwFoAUcEi7XgDA9Cb4xHTReNLETt+0clkwHQYDVR0OBBYEFLQI1Hig4yPUm6xIygThkbr60X8wMIGGBggrBgEFBQcBAwR6MHgwCgYGBACORgEBDAAwEwYGBACORgEGMAkGBwQAjkYBBgIwVQYGBACBmCcCMEswOTARBgcEAIGYJwEDDAZQU1BfQUkwEQYHBACBmCcBAQwGUFNQX0FTMBEGBwQAgZgnAQIMBlBTUF9QSQwGWC1XSU5HDAZOTC1YV0cwDQYJKoZIhvcNAQELBQADggEBAEW0Rq1KsLZooH27QfYQYy2MRpttoubtWFIyUV0Fc+RdIjtRyuS6Zx9j8kbEyEhXDi1CEVVeEfwDtwcw5Y3w6Prm9HULLh4yzgIKMcAsDB0ooNrmDwdsYcU/Oju23ym+6rWRcPkZE1on6QSkq8avBfrcxSBKrEbmodnJqUWeUv+oAKKG3W47U5hpcLSYKXVfBK1J2fnk1jxdE3mWeezoaTkGMQpBBARN0zMQGOTNPHKSsTYbLRCCGxcbf5oy8nHTfJpW4WO6rK8qcFTDOWzsW0sRxYviZFAJd8rRUCnxkZKQHIxeJXNQrrNrJrekLH3FbAm/LkyWk4Mw1w0TnQLAq+s=-----END CERTIFICATE-----" \
            // -H "authorization: Signature keyId=\"$keyId\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"$signature\"" \
            // -d "${payload}" \
            // --cert "${certPath}example_client_tls.cer" \
            // --key "${certPath}example_client_tls.key"      
            using (var client = HttpHelper.GetHttpClient(CertificatesPath))
            {
                var request = HttpHelper.GetHttpRequest(ServiceHost, HttpMethod.Post, reqPath, digest, reqDate, payload);
                request.Headers.Add("authorization", $"Signature keyId=\"{KeyId}\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"{signature}\"");
                request.Headers.Add("TPP-Signature-Certificate", "-----BEGIN CERTIFICATE-----MIIENjCCAx6gAwIBAgIEXkKZvjANBgkqhkiG9w0BAQsFADByMR8wHQYDVQQDDBZBcHBDZXJ0aWZpY2F0ZU1lYW5zQVBJMQwwCgYDVQQLDANJTkcxDDAKBgNVBAoMA0lORzESMBAGA1UEBwwJQW1zdGVyZGFtMRIwEAYDVQQIDAlBbXN0ZXJkYW0xCzAJBgNVBAYTAk5MMB4XDTIwMDIxMDEyMTAzOFoXDTIzMDIxMTEyMTAzOFowPjEdMBsGA1UECwwUc2FuZGJveF9laWRhc19xc2VhbGMxHTAbBgNVBGEMFFBTRE5MLVNCWC0xMjM0NTEyMzQ1MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkJltvbEo4/SFcvtGiRCar7Ah/aP0pY0bsAaCFwdgPikzFj+ij3TYgZLykz40EHODtG5Fz0iZD3fjBRRM/gsFPlUPSntgUEPiBG2VUMKbR6P/KQOzmNKF7zcOly0JVOyWcTTAi0VAl3MEO/nlSfrKVSROzdT4Aw/h2RVy5qlw66jmCTcp5H5kMiz6BGpG+K0dxqBTJP1WTYJhcEj6g0r0SYMnjKxBnztuhX5XylqoVdUy1a1ouMXU8IjWPDjEaM1TcPXczJFhakkAneoAyN6ztrII2xQ5mqmEQXV4BY/iQLT2grLYOvF2hlMg0kdtK3LXoPlbaAUmXCoO8VCfyWZvqwIDAQABo4IBBjCCAQIwNwYDVR0fBDAwLjAsoCqgKIYmaHR0cHM6Ly93d3cuaW5nLm5sL3Rlc3QvZWlkYXMvdGVzdC5jcmwwHwYDVR0jBBgwFoAUcEi7XgDA9Cb4xHTReNLETt+0clkwHQYDVR0OBBYEFLQI1Hig4yPUm6xIygThkbr60X8wMIGGBggrBgEFBQcBAwR6MHgwCgYGBACORgEBDAAwEwYGBACORgEGMAkGBwQAjkYBBgIwVQYGBACBmCcCMEswOTARBgcEAIGYJwEDDAZQU1BfQUkwEQYHBACBmCcBAQwGUFNQX0FTMBEGBwQAgZgnAQIMBlBTUF9QSQwGWC1XSU5HDAZOTC1YV0cwDQYJKoZIhvcNAQELBQADggEBAEW0Rq1KsLZooH27QfYQYy2MRpttoubtWFIyUV0Fc+RdIjtRyuS6Zx9j8kbEyEhXDi1CEVVeEfwDtwcw5Y3w6Prm9HULLh4yzgIKMcAsDB0ooNrmDwdsYcU/Oju23ym+6rWRcPkZE1on6QSkq8avBfrcxSBKrEbmodnJqUWeUv+oAKKG3W47U5hpcLSYKXVfBK1J2fnk1jxdE3mWeezoaTkGMQpBBARN0zMQGOTNPHKSsTYbLRCCGxcbf5oy8nHTfJpW4WO6rK8qcFTDOWzsW0sRxYviZFAJd8rRUCnxkZKQHIxeJXNQrrNrJrekLH3FbAm/LkyWk4Mw1w0TnQLAq+s=-----END CERTIFICATE-----");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                return JsonDocument.
                    Parse(HttpHelper.GetHttpResponseContent(client, request)).RootElement.
                    GetProperty("access_token").
                    ToString();
            }
        }

        public string GetAuthUrl(string accessToken)
        {
            //keyId="5ca1ab1e-c0ca-c01a-cafe-154deadbea75" # client_id as provided in the documentation
            //certPath="./certs/" # path of the downloaded certificates and keys
            //httpHost="https://api.sandbox.ing.com"
            var keyId = "5ca1ab1e-c0ca-c01a-cafe-154deadbea75";

            //# httpMethod must be in lower code
            //httpMethod="get"
            //reqPath="/oauth2/authorization-server-url?scope=payment-accounts%3Abalances%3Aview%20payment-accounts%3Atransactions%3Aview&redirect_uri=https://www.example.com&country_code=NL"
            var httpMethod = "get";
            var reqPath = "/oauth2/authorization-server-url?scope=payment-accounts%3Abalances%3Aview%20payment-accounts%3Atransactions%3Aview&redirect_uri=https://www.example.com&country_code=NL";

            //# Digest value for an empty body
            //digest="SHA-256=47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU="
            var digest = GetDigest(string.Empty);

            //# Generated value of the application access token. Please note that the access token expires in 15 minutes
            //accessToken="eyJhbGciOiJkaXIiLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIiwia2lkIjoidHN0LTRjOGI1Mzk3LTFhYjgtNDFhOC1hNTViLWE3MTk5MTJiODNkMiIsImN0eSI6IkpXVCJ9..Xo4H4hjJwWciJ17IO1pPWA.5s75BA1sQQtzPK4XYAnGnjDGuUDaLHRNLaLIrQd_WxSbV2pIJwSHpFsbrmsyHqNOuc2fgvx_tZwg5c-TjH8L0o_nnIsuxRlMhqnK9kLjY-tS0cK1tDBhTXmxyh2Bner8s7LGREAaJB-LAPxu_VO2ZpOjT-UnrgxZps-GeaZ4_NNPIBQKE7xV_2i9e5wrGf7C9vFzT02eINxGnj3tdd4V3Q58WIVsY4mSHEBPGA_yzw1V0z5qW_DuQvntIVbTO6sK42YFsX2MPIOJUrCfkg3mwVpZhmXB-UgxiioThBvDpg5evQhPaRKM_Y_bsLEMsN2eptDPIccEoxKtbx861OB3TfoGDO8Acxq9a_ZPEijmKQobwHOwAdrGL5liIEmNX7KkmX5T4ZmYP2B-_pxR2tkiMpDu5fPsomue9EhueYLapO3Hsi8r0mtTT5E3gZub7wz91g5V0SB-20ZR6A0nokTctT1NFhfU64paF5ZQJy7OHtnBOR2wLcTV3k75_6FCUGxckJwdL3aS05QU1p5O8gwQrxrrDMAUi2xfkD9UMJ22IoLDUjWSP44uhdhXAbgGzbYoK5_4R2viSRcfXybFYkp9ZTS87RZFs2WJXm8DohsdtWrvIpyEvznN2rZuJD_zbVpsKs3LQ1ZU0ni-KUyjVUwu0eJnjlgGprchH2DOc7VlZMvL9EZYLl9PeAnfXU_lFUHGe5jOjMTVNAOakDJ9etYZifwGexODg4R7bDJUCADoxmCEWZUfZHMWS2GnztDUADB3TQqUwNQ-aVkh3JvrEwg9YRYDF8IK1OS_UNrvxjNF0YXuGxUZxpKPnfdQL9X28JxQ-8tuTPzWQSIX-GIrtUT9yytLT24NHB8g4gr8rv7kDnoY0tscqvPLbJeM6hkt0Wyv84-9VqNtes7UQMRv6B34faj-fA70uPOW86A5ceia0JJUwps8SNh2JqCDvAG35ixWSUuEr5F2_60v1e19S_RvlrE1WaeKeOJmxugHxOWGpHUDrUUqkYWW93eRumMeXzPjadtGkHz0VW0gnvQPj3Mx-shNQEMzemCSK1N8s4Gc_L20oAmGbaqzGl_qPqacXMrDH8HT9vmLA7n1zj-JR1HR8LZJ4wNwFh86qo0pnOOkOrfSM14SOY28Nl4S-05r4PZfJheN4799_hVIW7VdYWPugpmKuEp27I5IsJibmHcEatoR0T47iIy5Ipj8qWekX8i0Dfap906XlqnxXCIJJCwopFF3PX0crwyNd-T7PlL2TBmEAxmYBsQFh0ZwSsJaz6MSTn6AIbdiFRbhk9zJ3N4qK5r6IDWeO-L_C9YIDwWukqxIMtLDN0Yip4M9Fy20sFuzt3W5m3uxC2CDpMqK-Si4_hbDo6Uc2Nf1F-NUQjsJCHOoI0k_il2dYYwZOXw5k0Hm1PCN2iGbGjkLMbOSZqnHY-J5OAp9XtSvCKsmlQYUq0qq7x6b2QC-HdD4VRto1U_ICeBpdXTqLdlnP6jqrs3fKLxgVnHOLBXp_UwLFEVQd-4kdBQtD7_fUbV5sQAsQBU6VH2Z1OypeTasW5Hr1F1haQHsafvj12D2R_vyRnzZTmWakN6tV4k0kPtiEEKP34uRI_OjnI4pDTRYqD0CcfGKSnBE4fJLihU6wZOfS3H6VPWkUMDV2eErUYnP0fj2QSLXIT4Wa68uSQg715ICaB-i3n0VlAu8wMx9ohoVmKWFZ2fHe5NgqDGIP2dIUtCUH7X1X_YtGACWeW4JHPU0oBmLiWhBNHjOZkPOG_czEz5PXFvKZYjxPLM0FB4sM4uweWGe2m8wgYaDzanKy_5quIK-OC_lQOxnM1u79Scz0IEMDGiqnOmmhFT6KqoR-SrgR6fT6O-VgYFgS3AOz6JN3YhcfVxr7lReN_ZXBv8eEYzuA2eSWSviGj5ISMMyqEyJsCa-Zr4IwKnIlM5smpvO-8pui1rDHybDh_bPjAWLLD9OFDVNxdz6KwyNT28f0DpGhSiT-qmsAzjQUmZsK-mnATDlR4pHh3Ng-iE27WNV6Jz0MyYaUlhxhaLK3RqPefGTOfxL-G2WooeI3s4v1s2Izxhp_kg28KU8S5XFRzjcpclGtznA36uZJQX2xdvNs8v9N9wLlu9nvjQoUNHUbODGHyQuGOEGBJlkziP49qDkJG1jxWPV6InLQiPBtRv_Edb4TQjxfW2yEsYRsySNqXH82PSkaZzBlrUPwe36YwGkghMd2qGulrVgQSvsktUe0hqF_40BjACtGQATzf-p3xfKHXdpOlz0x_sEOuvo_MHhCOChWG8sHgV4sBau__9zeGOGBKHfe5n9nUm0cij_fz8HcL8KZdib8gJfNs1yULaAPITuQRFYmKTMrUSlyTqs8bFRmoFmI4R_LbnUGpRdtlWWClTC5WdoU3uaAczqKWZD-PEl7ig1y2aNgbE1MsG3ugB6Yerw3K9isCmN6aZsdMypo1yx3gfw36qK7Dd8IFDxmlsD8iyn6C0iOQ432YV4SBDWzNtAC0W9uvyrX1oaxsvrTmds5XOIBj9vl70tImpmGb_qqc0gZdUgMq-BjtZAg3BatrgUU5b96HXMD7MgQNiOaG-3CgQBK-V9xCyRm9IePuq5ke4.TdCWauv20OjDu3Yz7r1rgSjqmpd4k4qf0wz43e6RD4E"

            //reqDate=$(LC_TIME=en_US.UTF-8 date -u "+%a, %d %b %Y %H:%M:%S GMT")
            var reqDate = GetDate();

            //# signingString must be declared exactly as shown below in separate lines
            //signingString="(request-target): $httpMethod $reqPath
            //date: $reqDate
            //digest: $digest"
            var signingString = GetSigningString(httpMethod, reqPath, digest, reqDate);

            //signature=`printf %s "$signingString" | openssl dgst -sha256 -sign "${certPath}example_client_signing.key" | openssl base64 -A`
            var signature = GetSignature(signingString, CertificatesPath);

            //# Curl request method must be in uppercase e.g "POST", "GET"
            //curl -i -X GET "${httpHost}${reqPath}" \
            //-H "Accept: application/json" \
            //-H "Content-Type: application/json" \
            //-H "Digest: ${digest}" \
            //-H "Date: ${reqDate}" \
            //-H "Authorization: Bearer ${accessToken}" \
            //-H "Signature: keyId=\"$keyId\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"$signature\"" \
            //-d "${payload}" \
            //--cert "${certPath}example_client_tls.cer" \
            //--key "${certPath}example_client_tls.key"
            using (var client = HttpHelper.GetHttpClient(CertificatesPath))
            {
                var request = HttpHelper.GetHttpRequest(ServiceHost, HttpMethod.Get, reqPath, digest, reqDate, string.Empty);
                request.Headers.Add("Authorization", $"Bearer {accessToken}");
                request.Headers.Add("Signature", $"keyId=\"{keyId}\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"{signature}\"");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                return JsonDocument.
                    Parse(HttpHelper.GetHttpResponseContent(client, request)).RootElement.
                    GetProperty("location").
                    ToString();
            }
        }

        public string GetBalance(string accessToken)
        {
            return Get(accessToken, "/v3/accounts/a217d676-7559-4f2a-83dc-5da0c2279223/balances?balanceTypes=interimBooked");
        }

        public string GetTransactions(string accessToken)
        {
            return Get(accessToken, "/v3/accounts/a217d676-7559-4f2a-83dc-5da0c2279223/transactions");
        }

        public string Get(string accessToken, string reqPath)
        {
            //keyId="5ca1ab1e-c0ca-c01a-cafe-154deadbea75" # client_id as provided in the documentation
            //certPath="./certs/" # path of the downloaded certificates and keys
            //httpHost="https://api.sandbox.ing.com"
            var keyId = "5ca1ab1e-c0ca-c01a-cafe-154deadbea75";

            //# httpMethod value must be in lower case
            //httpMethod="get"
            //reqPath="/v3/accounts/a217d676-7559-4f2a-83dc-5da0c2279223/balances?balanceTypes=interimBooked"
            var httpMethod = "get";

            //# Digest value for an empty body
            //digest="SHA-256=47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU="
            var digest = GetDigest(string.Empty);

            //# Generated value of the customer access token. Please note that the customer access token expires in 15 minutes
            //caccessToken="eyJhbGciOiJkaXIiLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIiwia2lkIjoidHN0LTRjOGI1Mzk3LTFhYjgtNDFhOC1hNTViLWE3MTk5MTJiODNkMiIsImN0eSI6IkpXVCJ9..ghsZyJmVO821V0g3-G3xhg.e7tlZxmaZrcDiJDvn74esOIXlSKaTf9JUWIOt6sf-uH8pGyDaAggZ1BRZmv9w-4okLAgO_QwNhGLO8SFECpHN-F5PhvU8hjjDUT68HLI0XXdL65hahZ4ph0MQYEQ2iVYjDpVXVfxwQYe36VILPDwK-imnliqCRrF5EXEtXGHplL0MLNTmxgbQuQoWmL4BwNR3yELYy11CtsWKhL7KTmQjsbKVF5aHmTO5oPp_0WIrq80YW-6sKspsLY5O97gg72WNFj_pbu_Mx7vTPFDesD7z6X7Ciu7voRMeo61j1jnB8YgThkpVH-PxGD7Km-jNqt-YE349onr_KfQvSfA5TI6JN9JiYlfHNOP2AoNm6IPpGTxAffMs41VXriM4ioEklSbNH8sr1Ah-vh__mdCm4i3sNM-T0Z28uveuZhL0glvOv5IhZZHBqWws52Jm2Wv6g4Vr_XaBUHPxWcoHgZjj3zAScxXjZmlC5-gBSfz50xgkCV9u5mtGcshzDLYH90-_l85Izz6Rxw_W4CdSZFzHVWRXktvqRnaZzqWJ2B-LzxxhjV3mKM25vqg4uG8bnjcCesy6d91nAtNI6PMMcLECFKzMx2gQY1e61C7-GIDz4w2gQhwNJN2MAW860j8MqdE85rh_hOHcFGW3gUi4HiAbaD65ccpj0t-R7b3lUmbDm5B2HN06dQAGV8QDonSALzSMGpYkd-bdATN9_3dJZ4Wd_UWUzJCokD78jBNphxHv6EHLHLUrw1i7CRyGOiwWEN0_OSkMO8Pnr51z8Oj58xrQr-XrP49PasAu1LRRxVJw48YdAxQhyxORzK1MR3EfPmcOGWoeeLStiZTW35dJIl6icf2lwSFXE7BumVUAW3k8m4b6S9LZnBmjRrAa-cwTAfdJ46gY-spXqL7kxGPHYQ9SQHfj6UHUJnOg1_j1JwUpersf3QSmJ01dHZ34iiFoYvLWs5HEM135lO89HQ8XBfv095pvDlDVH8RDZ8zw_uevlAbV7qCh660bCQeAqwque6o4hzexVtpbQDeHVtIwQ9FbvF8M6jnbm-ljxyDGMTc511U3MZ1wv5hZzaom0jXdb3LKVAwVeT-fdlBn0ykDNV6tOUtqe3GDFm526VxAN8TcAh36wSkN9cNNwdIFZU39j_SOzWUbyIrQbk-loNCyXiQb8OmE8VmQdrzjrJEiJ3_cv2bOLOdn4FH8-ag_5OAae9VM0XjWTHVpv88l3Bxb8Nf1snyrb2bDn-1KbpcJrjzpnSJW3nZ0FTBONBkVpfZ3VBNtOPjc-7-_WPTZIJb6k1GZJhFv5AsFXG40HaHl-pyWnQlXCXsaqNdVm4aLpGMFW-OIJkXBE5scmGyTvbHBZ0ac7iMm5vxknHU53hXbkD7Z4s1SKX9PchFYL6oy8LLCQpuqWGW3xJUOF9-ffiCqxcLNWL33vBG0D05iSjyBuT18G59gn9Q1c18anKcJEQIFuDnVecFfSmXXv1o9Bkv6v1Xc5nJ6ROSlQG3NVjAAANx4Nb8Kj2zObuKKFsM0jAYSJAdkLZqwIx2JCdR3MB_12DfpCxMwtI-8uwFiKQnOHQ2S52x_igs6x7NEE-9keioTqk9AHQWad9lXUW4FKoILK_Lvi6pEsHtprA5Co5u2MhKWsn5pJekmqI5xV4qdQ6jpp-uLVBmmVoObwtwJljdDKLcth2pvwh7AlcRdffBehjWN3Rs9sFeJPzBCeXy3RbVdLzVyRriYdxOd3UiIggaAc4gMo91tfUT6d80k44orzaYzzJS77xYOod8PY5qt_jYLFm3Ikda2i2Jp3AURqhUcXdFSNqbs5zCV3eKGOpRCKLFI6w3DDUo3Sc_DhdiSH7y4cekWZcKk_i24zdD9GgSZcJoP3HpCW3tF_3gi42GBGl_NX0TzvaQQ16mvQPnjJjnIE-tl1GVvhp6xf5ZkX2LQSb6KYkYNlUrifmi8Yjwsjd_P9iZKoU1I1dB3x6UWPwYk1WFYsPzRhMoMBICWQlhZFgFUaOlisZGIQ-L_u3cTCY7Wf6tWciUkcfZbAr7WzUC8QYO9ljbBf8KEil_mpFXM-2hjnx5T2IqjK_zIRw1h3IxTu1_Xo2-bSc6aPbOEaUZM0icL8pej86cRn8gYPb2K_GPlwQeAcQS8aRoHbW34ZhPdKSs8c4KygUlmCHP08nY2AI3vCGjcHcrmCnXmcNUsJSSbK1mfuZ46F8T1QvSdoZMkXQ6nZ2dzMhSIWlI9aSuCjcFEEGpGCQtJydcyb_mW4xhySJdqr_Tu2wvGTPZUmnuruElMJta1CDPvsWr7nATp5yzuqoy69XHkosz8V0VNbSTb3-AibBHQ5Y8Iiw8-h_NuazdwLcZnHWeoNQKOo4D_niu9HLwMWHYyybWwJrlxGZFb5MSv8L42ioY4sO6rAMJg9KlcNXxgcm0tQY7obORR7JW5rAsikOvnWMXoRdzpwEN5w5jL0CvKNzmaQ1Bjm48T7WVEE8YcCBULkyp-z0KlR-cGVO4FrEb-0URN_B3pG313ZQ7WiUSQHZxA3PCrb1H3JoViKDskN4Fg3hveGPO5hztywDV2s16tjmG8GwcVQTFIfEYB36yQuDHszTITPquKqmr7__8VqZzy60jmxG0673xdI4-YYP2RBDCDuweqFWvy8coOx8vcfahzjvVuU6s3r77g4nbMJLLxs39sjyKtKixkJQbEoeWbJKDZzd_Rdmqq6LFfM9BJMzvWI1TaBAh76zgvEdEtcIMWtU8af1rDQCnOi9mKBf3cYnA2WIzzjgkQAAXBghK8-aMGPxAci1CdE6zT0be1PzYuuuO3tQMVN1nhASydXcraEN78QWkRuhQS6uT7-2IO_972Bq8pwkQvrs1-QI_Ogkaf6unCv_aMQQPuZOtI54wq68cUX12eFA3Gl4vRWibNau7dhb_n0Pw6N96jiQ.Bct10Kov3BpDACdbbbk_zsjvjaLg4vaPPrJefOce4Co"

            //reqDate=$(LC_TIME=en_US.UTF-8 date -u "+%a, %d %b %Y %H:%M:%S GMT")
            var reqDate = GetDate();

            //# signingString must be declared exactly as shown below in separate lines
            //signingString="(request-target): $httpMethod $reqPath
            //date: $reqDate
            //digest: $digest"
            var signingString = GetSigningString(httpMethod, reqPath, digest, reqDate);

            //# signingString must be declared exactly as shown below in separate lines
            //signature=`printf %s "$signingString" | openssl dgst -sha256 -sign "${certPath}example_client_signing.key" | openssl base64 -A`
            var signature = GetSignature(signingString, CertificatesPath);

            //# Curl request method must be in uppercase e.g "POST", "GET"
            //curl -i -X GET "${httpHost}$reqPath" \
            //-H "Accept: application/json" \
            //-H "Content-Type: application/json" \
            //-H "Digest: ${digest}" \
            //-H "Date: ${reqDate}" \
            //-H "Authorization: Bearer ${caccessToken}" \
            //-H "Signature: keyId=\"$keyId\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"$signature\"" \
            //--cert "${certPath}example_client_tls.cer" \
            //--key "${certPath}example_client_tls.key"
            using (var client = HttpHelper.GetHttpClient(CertificatesPath))
            {
                var request = HttpHelper.GetHttpRequest(ServiceHost, HttpMethod.Get, reqPath, digest, reqDate, string.Empty);
                request.Headers.Add("X-Request-ID", $"{Guid.NewGuid()}");
                request.Headers.Add("Authorization", $"Bearer {accessToken}");
                request.Headers.Add("Signature", $"keyId=\"{keyId}\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"{signature}\"");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                return HttpHelper.GetHttpResponseContent(client, request);
            }
        }



        private static string GetSignature(string signingString, string certPath)
        {
            string signature;
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(Helper.GetBytesFromPEM(File.ReadAllText($"{certPath}example_client_signing.key"), PemStringType.RsaPrivateKey), out _);
                var data = Encoding.UTF8.GetBytes(signingString);
                signature = Convert.ToBase64String(rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
            }

            return signature;
        }

        private static string GetSigningString(string httpMethod, string reqPath, string digest, string reqDate)
        {
            // var reqDate = "Fri, 04 Mar 2022 21:45:52 GMT";

            // # signingString must be declared exactly as shown below in separate lines
            // signingString="(request-target): $httpMethod $reqPath
            // date: $reqDate
            // digest: $digest"
            return $"(request-target): {httpMethod} {reqPath}{'\n'}date: {reqDate}{'\n'}digest: {digest}";
        }

        private static string GetDate()
        {
            return DateTime.UtcNow.ToString(
                "ddd',' dd MMM yyyy HH':'mm':'ss 'GMT'",
                System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        private static string GetDigest(string payload)
        {
            var payloadDigest = Convert.ToBase64String(Helper.GetSHA256(Encoding.UTF8.GetBytes(payload)));
            var digest = $"SHA-256={payloadDigest}";
            return digest;
        }
    }
}
