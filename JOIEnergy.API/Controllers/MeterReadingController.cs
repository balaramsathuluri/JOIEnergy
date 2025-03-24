using JOIEnergy.Domain.Models;
using JOIEnergy.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JOIEnergy.API.Controllers
{
    [ApiController]
    [Route("readings")]
    public class MeterReadingController : ControllerBase
    {
        private readonly ILogger<MeterReadingController> _logger;
        private readonly IMeterReadingService _meterReadingService;

        public MeterReadingController(IMeterReadingService meterReadingService, ILogger<MeterReadingController> logger)
        {
            _meterReadingService = meterReadingService;
            _logger = logger;
        }

        [HttpPost("store")]
        public IActionResult StoreReading([FromBody] SmartMeterReadings meterReadings)
        {
            _logger.LogInformation("Storing readings for SmartMeterId: {SmartMeterId}", meterReadings?.SmartMeterId);
            try
            {
                if (!IsMeterReadingsValid(meterReadings))
                {
                    _logger.LogWarning("Invalid meter readings received.");
                    return BadRequest("Invalid meter readings. Ensure SmartMeterId and ElectricityReadings are provided.");
                }

                _meterReadingService.StoreReadings(meterReadings.SmartMeterId, meterReadings.ElectricityReadings);
                var storedReadings = _meterReadingService.GetReadings(meterReadings.SmartMeterId);

                return Ok(new { smartMeterId = meterReadings.SmartMeterId, electricityReadings = storedReadings });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error storing readings for SmartMeterId: {SmartMeterId}", meterReadings?.SmartMeterId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("read/{smartMeterId}")]
        public IActionResult GetReading(string smartMeterId)
        {
            _logger.LogInformation("Fetching readings for SmartMeterId: {SmartMeterId}", smartMeterId);
            try
            {
                var readings = _meterReadingService.GetReadings(smartMeterId);

                if (readings == null || !readings.Any())
                {
                    _logger.LogWarning("No readings found for SmartMeterId: {SmartMeterId}", smartMeterId);
                    return NotFound($"No readings found for smart meter: {smartMeterId}");
                }

                return Ok(new { smartMeterId, electricityReadings = readings });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching readings for SmartMeterId: {SmartMeterId}", smartMeterId);
                return StatusCode(500, "An error occurred while fetching meter readings.");
            }
        }

        private bool IsMeterReadingsValid(SmartMeterReadings meterReadings)
        {
            try
            {
                bool isValid = meterReadings != null &&
                               !string.IsNullOrWhiteSpace(meterReadings.SmartMeterId) &&
                               meterReadings.ElectricityReadings != null &&
                               meterReadings.ElectricityReadings.Any();
                if (!isValid)
                {
                    _logger.LogWarning("Meter readings validation failed.");
                }
                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating meter readings.");
                return false;
            }
        }
    }
}

