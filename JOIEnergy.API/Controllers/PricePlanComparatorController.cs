
using JOIEnergy.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JOIEnergy.API.Controllers
{
    [ApiController]
    [Route("price-plans")]
    public class PricePlanComparatorController : ControllerBase
    {
        private readonly ILogger<PricePlanComparatorController> _logger;
        private readonly IPricePlanService _pricePlanService;
        private readonly IAccountService _accountService;

        public PricePlanComparatorController(IPricePlanService pricePlanService, IAccountService accountService, ILogger<PricePlanComparatorController> logger)
        {
            _pricePlanService = pricePlanService;
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet("compare-all/{smartMeterId}")]
        public IActionResult CalculatedCostForEachPricePlan(string smartMeterId)
        {
            _logger.LogInformation("Calculating cost for SmartMeterId: {SmartMeterId}", smartMeterId);
            try
            {
                if (string.IsNullOrWhiteSpace(smartMeterId))
                {
                    _logger.LogWarning("Smart meter ID is null or empty");
                    return BadRequest("Smart meter ID cannot be null or empty.");
                }

                string pricePlanId = _accountService.GetPricePlanIdForSmartMeterId(smartMeterId);
                var costPerPricePlan = _pricePlanService.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);

                if (costPerPricePlan == null || !costPerPricePlan.Any())
                {
                    _logger.LogWarning("No cost data found for SmartMeterId: {SmartMeterId}", smartMeterId);
                    return NotFound($"Smart Meter ID ({smartMeterId}) not found");
                }

                var response = new Dictionary<string, object>
                {
                    { "pricePlanId", pricePlanId },
                    { "pricePlanComparisons", costPerPricePlan }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calculating price plan costs for SmartMeterId: {SmartMeterId}", smartMeterId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("recommend/{smartMeterId}")]
        public IActionResult RecommendCheapestPricePlans(string smartMeterId, int? limit = null)
        {
            _logger.LogInformation("Recommending cheapest price plan for SmartMeterId: {SmartMeterId}", smartMeterId);
            try
            {
                if (string.IsNullOrWhiteSpace(smartMeterId))
                {
                    _logger.LogWarning("Smart meter ID is null or empty");
                    return BadRequest("Smart meter ID cannot be null or empty.");
                }

                var consumptionForPricePlans = _pricePlanService.GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);
                if (consumptionForPricePlans == null || !consumptionForPricePlans.Any())
                {
                    _logger.LogWarning("No consumption data found for SmartMeterId: {SmartMeterId}", smartMeterId);
                    return NotFound($"Smart Meter ID ({smartMeterId}) not found");
                }

                var recommendations = consumptionForPricePlans.OrderBy(x => x.Value);
                if (limit.HasValue && limit.Value > 0)
                {
                    return Ok(recommendations.Take(limit.Value));
                }

                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while recommending price plans for SmartMeterId: {SmartMeterId}", smartMeterId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
