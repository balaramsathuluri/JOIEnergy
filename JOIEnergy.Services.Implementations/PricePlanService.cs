using JOIEnergy.Domain.Exceptions;
using JOIEnergy.Domain.Models;
using JOIEnergy.Repository.Interfaces;
using JOIEnergy.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace JOIEnergy.Services.Implementations
{
    public class PricePlanService : IPricePlanService
    {
        #region Private Members

        private readonly ILogger<PricePlanService> _logger;
        private readonly List<PricePlan> _pricePlans;
        private readonly IMeterReadingService _meterReadingService;

        #endregion

        #region Constructor
        public PricePlanService(IPricePlanRepository pricePlanRepository, IMeterReadingService meterReadingService, ILogger<PricePlanService> logger)
        {
            _pricePlans = pricePlanRepository.GetPricePlans();
            _meterReadingService = meterReadingService;
            _logger = logger;
        }

        #endregion

        #region Private Methods


        private decimal CalculateAverageReading(List<ElectricityReading> electricityReadings)
        {
            try
            {
                _logger.LogInformation("Calculating average reading for {Count} readings", electricityReadings.Count);
                return electricityReadings.Count == 0 ? 0 : electricityReadings.Average(r => r.Reading);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating average reading.");
                return 0;
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

                if (timeElapsed == 0)
                {
                    _logger.LogWarning("Time elapsed is zero, avoiding division by zero.");
                    return 0; 
                }

                var averagedCost = average / timeElapsed;
                return Math.Round(averagedCost * pricePlan.UnitRate, 3);
            }
            catch (Exception ex)
            {
                throw new CalculationException($"Error calculating cost for {pricePlan.PlanName}", ex);                
            }
        }


        #endregion

        #region Public Methods
        public Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId)
        {
            _logger.LogInformation("Calculating consumption cost for SmartMeterId: {SmartMeterId}", smartMeterId);

            try
            {
                List<ElectricityReading> electricityReadings = _meterReadingService.GetReadings(smartMeterId);

                if (!electricityReadings.Any())
                {
                    _logger.LogWarning("No readings found for SmartMeterId: {SmartMeterId}. Check if the meter exists or if data is missing.", smartMeterId);
                    return new Dictionary<string, decimal>();
                }

                var costByPlan = new Dictionary<string, decimal>();

                foreach (var plan in _pricePlans)
                {
                    try
                    {
                        costByPlan[plan.PlanName] = CalculateCost(electricityReadings, plan);
                    }
                    catch (CalculationException calcEx)
                    {
                        _logger.LogError(calcEx, "Calculation error for PricePlan: {PricePlan} and SmartMeterId: {SmartMeterId}", plan.PlanName, smartMeterId);
                        costByPlan[plan.PlanName] = -1; // Default or fallback value, can be adjusted
                    }
                }

                return costByPlan;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while calculating consumption cost for SmartMeterId: {SmartMeterId}", smartMeterId);
                throw;
            }
        }

        #endregion
    }
}

