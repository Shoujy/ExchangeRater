namespace ExchangeRater.Models;

public class EstimateRequest
{
    public decimal InputAmount { get; set; }
    public string InputCurrency { get; set; } = String.Empty;
    public string OutputCurrency { get; set; } = String.Empty;
}