using Microsoft.Extensions.Logging;
using JOIEnergy.Repository.Interfaces;
using JOIEnergy.Services.Interfaces;
using System;

namespace JOIEnergy.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly ISmartMeterPricePlanRepository _smartMeterPricePlanRepository;

        public AccountService(ISmartMeterPricePlanRepository smartMeterPricePlanRepository, ILogger<AccountService> logger)
        {
            _smartMeterPricePlanRepository = smartMeterPricePlanRepository;
            _logger = logger;
        }

        public string? GetPricePlanIdForSmartMeterId(string smartMeterId)
        {
            _logger.LogInformation("GetPricePlanIdForSmartMeterId started");
            try
            {
                if (string.IsNullOrEmpty(smartMeterId))
                {
                    _logger.LogWarning("SmartMeterId is null or empty");
                    return null;
                }

                if (_smartMeterPricePlanRepository.GetSmartMeterToPricePlanMappings().TryGetValue(smartMeterId, out var planId))
                {
                    _logger.LogInformation("Successfully retrieved price plan ID");
                    return planId;
                }

                _logger.LogWarning("No price plan found for given SmartMeterId");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPricePlanIdForSmartMeterId");
                throw;
            }
        }
    }
}
