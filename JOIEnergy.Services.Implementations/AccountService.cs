using JOIEnergy.Repository.Interfaces;
using JOIEnergy.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace JOIEnergy.Services.Implementations
{
    public class AccountService : IAccountService
    {
        #region Private Members

        private readonly ILogger<AccountService> _logger;
        private readonly ISmartMeterPricePlanRepository _smartMeterPricePlanRepository;

        #endregion

        #region Constructor
        public AccountService(ISmartMeterPricePlanRepository smartMeterPricePlanRepository, ILogger<AccountService> logger)
        {
            _smartMeterPricePlanRepository = smartMeterPricePlanRepository;
            _logger = logger;
        }
        #endregion

        #region Public Methods
        public string? GetPricePlanIdForSmartMeterId(string smartMeterId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(smartMeterId))
                {
                    _logger.LogWarning("SmartMeterId is null or empty. Cannot retrieve price plan.");
                    return null;
                }

                _logger.LogInformation("Retrieving price plan for SmartMeterId: {SmartMeterId}", smartMeterId);

                if (_smartMeterPricePlanRepository.GetSmartMeterToPricePlanMappings()
                    .TryGetValue(smartMeterId, out var planId))
                {
                    _logger.LogInformation("Successfully retrieved price plan ID: {PricePlanId} for SmartMeterId: {SmartMeterId}", planId, smartMeterId);
                    return planId;
                }

                _logger.LogWarning("No price plan found for SmartMeterId: {SmartMeterId}", smartMeterId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving price plan for SmartMeterId: {SmartMeterId}", smartMeterId);
                throw;
            }
        }


    }
    #endregion
}
