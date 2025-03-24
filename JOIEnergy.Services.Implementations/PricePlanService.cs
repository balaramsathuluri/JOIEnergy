using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;
using JOIEnergy.Services.Interfaces;
using JOIEnergy.Repository.Interfaces;
using JOIEnergy.Domain.Models;

namespace JOIEnergy.Services.Implementations
{
    public class PricePlanService : IPricePlanService
    {
        private readonly List<PricePlan> _pricePlans;
        private readonly IMeterReadingService _meterReadingService;

        public PricePlanService(IPricePlanRepository pricePlanRepository, IMeterReadingService meterReadingService)
        {
            // Retrieve price plans from repository (read from JSON)
            _pricePlans = pricePlanRepository.GetPricePlans();
            _meterReadingService = meterReadingService;
        }

        /// <summary>
        /// Calculates the average of all electricity readings.
        /// </summary>
        private decimal CalculateAverageReading(List<ElectricityReading> electricityReadings)
        {
            var summedReadings = electricityReadings.Select(r => r.Reading)
                                                     .Aggregate((acc, reading) => acc + reading);
            return summedReadings / electricityReadings.Count;
        }

        /// <summary>
        /// Calculates the elapsed time in hours between the earliest and latest reading.
        /// </summary>
        private decimal CalculateTimeElapsed(List<ElectricityReading> electricityReadings)
        {
            var first = electricityReadings.Min(r => r.Time);
            var last = electricityReadings.Max(r => r.Time);
            return (decimal)(last - first).TotalHours;
        }

        /// <summary>
        /// Calculates the consumption cost for a given price plan.
        /// </summary>
        private decimal CalculateCost(List<ElectricityReading> electricityReadings, PricePlan pricePlan)
        {
            var average = CalculateAverageReading(electricityReadings);
            var timeElapsed = CalculateTimeElapsed(electricityReadings);
            var averagedCost = average / timeElapsed;
            return Math.Round(averagedCost * pricePlan.UnitRate, 3);
        }

        /// <summary>
        /// Returns a dictionary with consumption costs for each price plan.
        /// </summary>
        public Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId)
        {
            List<ElectricityReading> electricityReadings = _meterReadingService.GetReadings(smartMeterId);
            if (!electricityReadings.Any())
            {
                return new Dictionary<string, decimal>();
            }

            return _pricePlans.ToDictionary(plan => plan.PlanName, plan => CalculateCost(electricityReadings, plan));
        }

        /// <summary>
        /// (Optional) Recommends the cheapest price plan for the given smart meter.
        /// </summary>
        public string RecommendCheapestPricePlan(string smartMeterId)
        {
            var costs = GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);
            if (!costs.Any())
            {
                return null;
            }
            return costs.OrderBy(kvp => kvp.Value).First().Key;
        }
    }
}
