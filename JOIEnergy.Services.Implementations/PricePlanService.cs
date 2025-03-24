using JOIEnergy.Domain.Models;
using JOIEnergy.Repository.Interfaces;
using JOIEnergy.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace JOIEnergy.Services.Implementations
{
    public class PricePlanService : IPricePlanService
    {
        private readonly ILogger<PricePlanService> _logger;
        private readonly List<PricePlan> _pricePlans;
        private readonly IMeterReadingService _meterReadingService;

        public PricePlanService(IPricePlanRepository pricePlanRepository, IMeterReadingService meterReadingService, ILogger<PricePlanService> logger)
        {
            _pricePlans = pricePlanRepository.GetPricePlans();
            _meterReadingService = meterReadingService;
            _logger = logger;
        }

        private decimal CalculateAverageReading(List<ElectricityReading> electricityReadings)
        {
            try
            {
                _logger.LogInformation("Calculating average reading");
                var summedReadings = electricityReadings.Select(r => r.Reading).Aggregate((acc, reading) => acc + reading);
                return summedReadings / electricityReadings.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating average reading");
                throw;
            }
        }

        private decimal CalculateTimeElapsed(List<ElectricityReading> electricityReadings)
        {
            try
            {
                _logger.LogInformation("Calculating time elapsed");
                var first = electricityReadings.Min(r => r.Time);
                var last = electricityReadings.Max(r => r.Time);
                return (decimal)(last - first).TotalHours;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating time elapsed");
                throw;
            }
        }

        private decimal CalculateCost(List<ElectricityReading> electricityReadings, PricePlan pricePlan)
        {
            try
            {
                _logger.LogInformation("Calculating cost for price plan: {PricePlan}", pricePlan.PlanName);
                var average = CalculateAverageReading(electricityReadings);
                var timeElapsed = CalculateTimeElapsed(electricityReadings);
                var averagedCost = average / timeElapsed;
                return Math.Round(averagedCost * pricePlan.UnitRate, 3);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating cost for price plan: {PricePlan}", pricePlan.PlanName);
                throw;
            }
        }

        public Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId)
        {
            _logger.LogInformation("Calculating consumption cost for SmartMeterId: {SmartMeterId}", smartMeterId);
            try
            {
                List<ElectricityReading> electricityReadings = _meterReadingService.GetReadings(smartMeterId);
                if (!electricityReadings.Any())
                {
                    _logger.LogWarning("No readings found for SmartMeterId: {SmartMeterId}", smartMeterId);
                    return new Dictionary<string, decimal>();
                }
                return _pricePlans.ToDictionary(plan => plan.PlanName, plan => CalculateCost(electricityReadings, plan));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating consumption cost for SmartMeterId: {SmartMeterId}", smartMeterId);
                throw;
            }
        }
    }
}

