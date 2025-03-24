using JOIEnergy.Repository.Interfaces;
using JOIEnergy.Services.Interfaces;

namespace JOIEnergy.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly ISmartMeterPricePlanRepository _smartMeterPricePlanRepository;

        public AccountService(ISmartMeterPricePlanRepository smartMeterPricePlanRepository)
        {
            _smartMeterPricePlanRepository = smartMeterPricePlanRepository;
        }

        public string? GetPricePlanIdForSmartMeterId(string smartMeterId)
        {

            if (string.IsNullOrEmpty(smartMeterId))
            {
                return null;
            }

            if (_smartMeterPricePlanRepository.GetSmartMeterToPricePlanMappings().TryGetValue(smartMeterId, out var planId))
            {
                return planId;
            }
            return null;
        }
    }
}
