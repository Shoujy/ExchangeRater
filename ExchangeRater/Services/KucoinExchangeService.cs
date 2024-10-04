using Newtonsoft.Json.Linq;

namespace ExchangeRater.Services;

public class KucoinExchangeService : ICryptoExchangeService
{
    private readonly HttpClient _httpClient;

    public KucoinExchangeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal?> GetRatesAsync(string baseCurrency, string quoteCurrency)
    {
        string symbol = $"{baseCurrency}-{quoteCurrency}".ToUpper();
    
        try
        {
            var response = await _httpClient.GetAsync($"https://api.kucoin.com/api/v1/market/orderbook/level1?symbol={symbol}");
        
            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
        
            if (json["code"]?.ToString() != "200000")
            {
                Console.WriteLine($"KuCoin API returned an error: {json["msg"]?.ToString() ?? "Unknown error"}");
                return null;
            }
        
            var data = json["data"];
            if (data == null || data["price"] == null)
            {
                Console.WriteLine("Price information is missing from the KuCoin API response.");
                return null;
            }
        
            var price = data["price"].Value<decimal>();
            return price;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return null;
        }
    }

    public string GetExchangeName()
    {
        return "KuCoin";
    }
}