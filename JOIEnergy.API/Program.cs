using JOIEnergy.Repository.Implementations;
using JOIEnergy.Repository.Interfaces;
using JOIEnergy.Services.Implementations;
using JOIEnergy.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

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

// Add HTTP Logging with configuration
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;
});


/// Register Repositories
builder.Services.AddSingleton<IUserRepository, UserRepository>();
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
app.UseHttpLogging();
app.UseAuthorization();

// Group API Routes for Better Organization (New in .NET 8)
//var apiGroup = app.MapGroup("/api");
app.MapControllers();

app.Run();
