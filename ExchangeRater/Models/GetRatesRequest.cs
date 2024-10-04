namespace ExchangeRater.Models;

public class GetRatesRequest
{
    public string BaseCurrency { get; set; } = String.Empty;
    public string QuoteCurrency { get; set; } = String.Empty;
}