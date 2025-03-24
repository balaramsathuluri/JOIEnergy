
using JOIEnergy.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JOIEnergy.API.Controllers
{
    [ApiController]
    [Route("price-plans")]
    public class PricePlanComparatorController : ControllerBase
    {
        private readonly IPricePlanService _pricePlanService;
        private readonly IAccountService _accountService;

        public PricePlanComparatorController(IPricePlanService pricePlanService, IAccountService accountService)
        {
            _pricePlanService = pricePlanService;
            _accountService = accountService;
        }

        /// <summary>
        /// Compares consumption costs for each price plan for a given smart meter.
        /// Returns an object containing the current price plan and the cost comparisons.
        /// </summary>
        /// <param name="smartMeterId">Smart meter identifier.</param>
        /// <returns>Price plan comparison details.</returns>
        [HttpGet("compare-all/{smartMeterId}")]
        public IActionResult CalculatedCostForEachPricePlan(string smartMeterId)
        {
            if (string.IsNullOrWhiteSpace(smartMeterId))
            {
                return BadRequest("Smart meter ID cannot be null or empty.");
            }

            string pricePlanId = _accountService.GetPricePlanIdForSmartMeterId(smartMeterId);
            var costPerPricePlan = _pricePlanService.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);

            if (costPerPricePlan == null || !costPerPricePlan.Any())
            {
                return NotFound($"Smart Meter ID ({smartMeterId}) not found");
            }

            var response = new Dictionary<string, object>
            {
                { "pricePlanId", pricePlanId },
                { "pricePlanComparisons", costPerPricePlan }
            };

            return Ok(response);
        }

        /// <summary>
        /// Recommends the cheapest price plans based on consumption cost.
        /// Optionally limits the number of recommendations returned.
        /// </summary>
        /// <param name="smartMeterId">Smart meter identifier.</param>
        /// <param name="limit">Optional limit for the number of recommendations.</param>
        /// <returns>A sorted list of price plan recommendations.</returns>
        [HttpGet("recommend/{smartMeterId}")]
        public IActionResult RecommendCheapestPricePlans(string smartMeterId, int? limit = null)
        {
            if (string.IsNullOrWhiteSpace(smartMeterId))
            {
                return BadRequest("Smart meter ID cannot be null or empty.");
            }

            var consumptionForPricePlans = _pricePlanService.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);
            if (consumptionForPricePlans == null || !consumptionForPricePlans.Any())
            {
                return NotFound($"Smart Meter ID ({smartMeterId}) not found");
            }

            var recommendations = consumptionForPricePlans.OrderBy(x => x.Value);

            if (limit.HasValue && limit.Value > 0)
            {
                var limitedRecommendations = recommendations.Take(limit.Value);
                return Ok(limitedRecommendations);
            }

            return Ok(recommendations);
        }

    }
}
