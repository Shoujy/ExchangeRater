namespace ExchangeRater.Services;

public interface ICryptoExchangeService
{
    Task<decimal?> GetRatesAsync(string baseCurrency, string quoteCurrency);
    string GetExchangeName();
}