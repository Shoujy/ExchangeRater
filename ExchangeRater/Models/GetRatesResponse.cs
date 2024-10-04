namespace ExchangeRater.Models;

public class GetRatesResponse
{
    public string ExchangeName { get; set; } = String.Empty;
    public decimal? Rate { get; set; }
}