namespace IngConnector
{
    using Microsoft.Extensions.Configuration;
    using System;

    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public static void Main(string[] args)
        {
            // Load configuration
            var configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).
                AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true).
                Build();

            //// TODO download certificates
            //https://developer.ing.com/openbanking/assets/data/psd2/example_client_signing.cer
            //https://developer.ing.com/openbanking/assets/data/psd2/example_client_signing.key
            //https://developer.ing.com/openbanking/assets/data/psd2/example_client_tls.cer
            //https://developer.ing.com/openbanking/assets/data/psd2/example_client_tls.key

            // Get service instance
            var service = new IngSandboxService(configuration);

            // Get token from service
            var accessToken = service.GetAccessToken();
            Console.WriteLine($"accessToken:{accessToken}");
            Console.WriteLine();

            //var authUrl = service.GetAuthUrl(accessToken);
            //Console.WriteLine($"{authUrl}?&client_id=5ca1ab1e-c0ca-c01a-cafe-154deadbea75&state=ANY_ARBITRARY_VALUE&redirect_uri=https%3A%2F%2Fwww.example.com%2F&scope=payment-accounts%3Abalances%3Aview%20payment-accounts%3Atransactions%3Aview");
            //Console.WriteLine();

            ////Process.Start($"{authUrl}?&client_id=5ca1ab1e-c0ca-c01a-cafe-154deadbea75&state=ANY_ARBITRARY_VALUE&redirect_uri=https%3A%2F%2Fwww.example.com%2F&scope=payment-accounts%3Abalances%3Aview%20payment-accounts%3Atransactions%3Aview");
            //var authCode = Console.ReadLine();
            //Console.WriteLine($"authCode:{authCode}");
            //Console.WriteLine();

            // Invoke check balance from service
            var balance = service.GetBalance(accessToken);
            Console.WriteLine($"balance:{balance}");
            Console.WriteLine();

            // Invoke get transactions from service
            var transactions = service.GetTransactions(accessToken);
            Console.WriteLine($"transactions:{transactions}");
            Console.WriteLine();
        }
    }
}
