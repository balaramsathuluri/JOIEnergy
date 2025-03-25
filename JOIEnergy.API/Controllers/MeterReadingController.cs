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
        #region Private Members

        private readonly ILogger<MeterReadingController> _logger;
        private readonly IMeterReadingService _meterReadingService;

        #endregion


        #region Constructor
        public MeterReadingController(IMeterReadingService meterReadingService, ILogger<MeterReadingController> logger)
        {
            _meterReadingService = meterReadingService;
            _logger = logger;
        }

        #endregion

        #region API Methods

        [HttpPost("store")]
        public IActionResult StoreReading([FromBody] MeterReadings meterReadings)
        {
            _logger.LogInformation("Received request to store readings for SmartMeterId: {SmartMeterId}", meterReadings?.SmartMeterId ?? "NULL");

            try
            {
                if (!IsMeterReadingsValid(meterReadings))
                {
                    _logger.LogWarning("Invalid meter readings received for SmartMeterId: {SmartMeterId}", meterReadings?.SmartMeterId ?? "NULL");
                    return UnprocessableEntity("Invalid meter readings. Ensure SmartMeterId and ElectricityReadings are provided.");
                }

                _meterReadingService.StoreReadings(meterReadings.SmartMeterId, meterReadings.ElectricityReadings);
                var storedReadings = _meterReadingService.GetReadings(meterReadings.SmartMeterId);

                _logger.LogInformation("Successfully stored readings for SmartMeterId: {SmartMeterId}", meterReadings.SmartMeterId);
                return Ok(new { smartMeterId = meterReadings.SmartMeterId, electricityReadings = storedReadings });
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Missing required values for SmartMeterId: {SmartMeterId}", meterReadings?.SmartMeterId ?? "NULL");
                return BadRequest("SmartMeterId or readings cannot be null.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while storing readings for SmartMeterId: {SmartMeterId}", meterReadings?.SmartMeterId ?? "NULL");
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
                    _logger.LogWarning("No readings found for SmartMeterId: {SmartMeterId}. Check if the meter exists or if data is missing.", smartMeterId);

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

        #endregion

        private bool IsMeterReadingsValid(MeterReadings meterReadings)
        {
            if (meterReadings == null)
            {
                _logger.LogWarning("Validation failed: MeterReadings object is null.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(meterReadings.SmartMeterId))
            {
                _logger.LogWarning("Validation failed: SmartMeterId is missing or empty.");
                return false;
            }

            if (meterReadings.ElectricityReadings == null || !meterReadings.ElectricityReadings.Any())
            {
                _logger.LogWarning("Validation failed: ElectricityReadings are missing or empty.");
                return false;
            }

            return true;
        }

    }
}

