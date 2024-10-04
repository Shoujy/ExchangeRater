namespace ExchangeRater.Models;

public class EstimateResponse
{
    public string ExchangeName { get; set; } = String.Empty;
    public decimal OutputAmount { get; set; }
}