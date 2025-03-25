using JOIEnergy.Repository.Implementations;
using JOIEnergy.Repository.Interfaces;
using JOIEnergy.Services.Implementations;
using JOIEnergy.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Register logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();  // Console logging
    logging.AddDebug();    // Debug output logging
});

// Add Controllers & API Docs
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

/// Register Repositories
builder.Services.AddSingleton<IPricePlanRepository, PricePlanRepository>();
builder.Services.AddSingleton<IMeterReadingRepository, MeterReadingRepository>();
builder.Services.AddSingleton<ISmartMeterPricePlanRepository, SmartMeterPricePlanRepository>();

// Register Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IMeterReadingService, MeterReadingService>();
builder.Services.AddScoped<IPricePlanService, PricePlanService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply Middleware
app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthorization();


app.MapControllers();

app.Run();
