using Newtonsoft.Json.Linq;

namespace ExchangeRater.Services;

public class KrakenExchangeService : ICryptoExchangeService
{
    private readonly HttpClient _httpClient;

    public KrakenExchangeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string GetExchangeName()
    {
        return "Kraken";
    }

    public async Task<decimal?> GetRatesAsync(string baseCurrency, string quoteCurrency)
    {
        string pair = $"{baseCurrency}{quoteCurrency}".ToUpper();

        try
        {
            var response = await _httpClient.GetAsync($"https://api.kraken.com/0/public/Ticker?pair={pair}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching data from Kraken API for pair {pair}: {response.ReasonPhrase}");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            var priceToken = json["result"]?[pair]?["c"]?[0];

            if (priceToken == null)
            {
                Console.WriteLine($"Price information is missing from Kraken API response for pair {pair}.");
                return null;
            }

            return priceToken.Value<decimal>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An exception occurred while fetching rates from Kraken: {ex.Message}");
            return null;
        }
    }
}