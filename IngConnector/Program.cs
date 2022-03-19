
using IngConnector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

var logger = LoggerFactory.
    Create(builder => { builder.AddConsole(); }).
    CreateLogger<IngService>();
var configuration = new ConfigurationBuilder().
    AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).
    AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true).
    AddCommandLine(args).
    Build();

//// TODO download certificates
//https://developer.ing.com/openbanking/assets/data/psd2/example_client_signing.cer
//https://developer.ing.com/openbanking/assets/data/psd2/example_client_signing.key
//https://developer.ing.com/openbanking/assets/data/psd2/example_client_tls.cer
//https://developer.ing.com/openbanking/assets/data/psd2/example_client_tls.key

// Get service instance
IIngService service = new IngService(configuration, logger);

// Get token from service
var accessToken = service.GetAccessToken();
Console.WriteLine($"accessToken:{accessToken}{Environment.NewLine}");

// Invoke check balance from service
var balance = service.GetBalance(accessToken);
Console.WriteLine($"balance:{balance}{Environment.NewLine}");

// Invoke get transactions from service
var transactions = service.GetTransactions(accessToken);
Console.WriteLine($"transactions:{transactions}{Environment.NewLine}");