namespace IngConnector;

public interface IIngService
{
    string GetAccessToken();
    string GetBalance(string accessToken);
    string GetTransactions(string accessToken);
}
