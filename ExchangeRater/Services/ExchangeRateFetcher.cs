using ExchangeRater.Models;

namespace ExchangeRater.Services;

public class ExchangeRateFetcher
{
    private readonly IEnumerable<ICryptoExchangeService> _exchangeServices;

    public ExchangeRateFetcher(IEnumerable<ICryptoExchangeService> exchangeServices)
    {
        _exchangeServices = exchangeServices;
    }

    public async Task<List<EstimateResponse>> GetEstimatesAsync(string inputCurrency, string outputCurrency, decimal inputAmount)
    {
        List<EstimateResponse> estimates = new();

        foreach (var service in _exchangeServices)
        {
            var rate = await GetRateForPairAsync(service, inputCurrency, outputCurrency);
            if (rate == null)
            {
                Console.WriteLine($"Error fetching rate from {service.GetExchangeName()} for {inputCurrency}/{outputCurrency}");
                continue;
            }

            var outputAmount = inputAmount * rate.Value;
            estimates.Add(new EstimateResponse
            {
                ExchangeName = service.GetExchangeName(),
                OutputAmount = outputAmount
            });
        }

        return estimates;
    }

    public async Task<List<GetRatesResponse>> GetRatesAsync(string baseCurrency, string quoteCurrency)
    {
        List<GetRatesResponse> rates = new();

        foreach (var service in _exchangeServices)
        {
            var rate = await GetRateForPairAsync(service, baseCurrency, quoteCurrency);
            if (rate == null)
            {
                Console.WriteLine($"Error fetching rate from {service.GetExchangeName()} for {baseCurrency}/{quoteCurrency}");
                rates.Add(new GetRatesResponse
                {
                    ExchangeName = service.GetExchangeName(),
                    Rate = null
                });
                continue;
            }

            rates.Add(new GetRatesResponse
            {
                ExchangeName = service.GetExchangeName(),
                Rate = rate.Value
            });
        }

        return rates;
    }

    // Method to get the rate for a pair, including inverse rate fallback
    private async Task<decimal?> GetRateForPairAsync(ICryptoExchangeService service, string baseCurrency, string quoteCurrency)
    {
        // Try to get the rate for the direct pair
        decimal? directRate = await service.GetRatesAsync(baseCurrency, quoteCurrency);
        if (directRate != null)
        {
            return directRate;
        }

        // If direct rate is null, try the inverse pair
        decimal? inverseRate = await service.GetRatesAsync(quoteCurrency, baseCurrency);
        if (inverseRate != null)
        {
            return 1 / inverseRate;
        }
        
        return null;
    }
}