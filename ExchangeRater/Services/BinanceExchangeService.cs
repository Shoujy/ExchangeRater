using Newtonsoft.Json.Linq;

namespace ExchangeRater.Services;

public class BinanceExchangeService : ICryptoExchangeService
{
    private readonly HttpClient _httpClient;

    public BinanceExchangeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<decimal?> GetRatesAsync(string baseCurrency, string quoteCurrency)
    {
        string symbol = $"{baseCurrency}{quoteCurrency}".ToUpper();
    
        try
        {
            var response = await _httpClient.GetAsync($"https://api.binance.com/api/v3/ticker/price?symbol={symbol}");
    
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching data from Binance API. Status Code: {response.StatusCode}");
                return null;
            }
    
            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
    
            var priceToken = json["price"];
    
            if (priceToken == null)
            {
                Console.WriteLine("Price information is missing from the Binance API response.");
                return null; 
            }
    
            var price = priceToken.Value<decimal>();
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
        return "Binance";
    }
}