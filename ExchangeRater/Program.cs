using ExchangeRater.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add HttpClient services.
builder.Services.AddHttpClient<BinanceExchangeService>();
builder.Services.AddHttpClient<KucoinExchangeService>();
builder.Services.AddHttpClient<KrakenExchangeService>();

// Register the exchange services.
builder.Services.AddTransient<ICryptoExchangeService, BinanceExchangeService>();
builder.Services.AddTransient<ICryptoExchangeService, KucoinExchangeService>();
builder.Services.AddTransient<ICryptoExchangeService, KrakenExchangeService>();

builder.Services.AddScoped<ExchangeRateFetcher>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();