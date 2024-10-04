using ExchangeRater.Models;
using ExchangeRater.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRater.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeController : ControllerBase
{
    private readonly ExchangeRateFetcher _exchangeRateFetcher;

    public ExchangeController(ExchangeRateFetcher exchangeRateFetcher)
    {
        _exchangeRateFetcher = exchangeRateFetcher;
    }
    
    [HttpGet("estimate")]
    public async Task<IActionResult> Estimate([FromQuery] EstimateRequest request)
    {
        var estimates = await _exchangeRateFetcher.GetEstimatesAsync(request.InputCurrency, request.OutputCurrency, request.InputAmount);

        if (!estimates.Any())
        {
            return NotFound("No valid estimates found.");
        }

        var bestEstimate = estimates.OrderByDescending(e => e.OutputAmount).First();
        return Ok(bestEstimate);
    }
    
    [HttpGet("getRates")]
    public async Task<IActionResult> GetRates([FromQuery] GetRatesRequest request)
    {
        var rates = await _exchangeRateFetcher.GetRatesAsync(request.BaseCurrency, request.QuoteCurrency);
        return Ok(rates);
    }
}
