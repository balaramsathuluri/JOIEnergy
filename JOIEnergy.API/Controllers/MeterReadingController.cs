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
        private readonly IMeterReadingService _meterReadingService;

        public MeterReadingController(IMeterReadingService meterReadingService)
        {
            _meterReadingService = meterReadingService;
        }

        /// <summary>
        /// Stores meter readings and returns the stored data.
        /// </summary>
        /// <param name="meterReadings">The meter readings payload containing the smart meter ID and electricity readings.</param>
        /// <returns>The stored readings in the required format.</returns>
        [HttpPost("store")]
        public IActionResult StoreReading([FromBody] SmartMeterReadings meterReadings)
        {
            if (!IsMeterReadingsValid(meterReadings))
            {
                return BadRequest("Invalid meter readings. Ensure SmartMeterId and ElectricityReadings are provided.");
            }

            _meterReadingService.StoreReadings(meterReadings.SmartMeterId, meterReadings.ElectricityReadings);

            // Retrieve the stored readings to return a complete response.
            var storedReadings = _meterReadingService.GetReadings(meterReadings.SmartMeterId);

            var response = new
            {
                smartMeterId = meterReadings.SmartMeterId,
                electricityReadings = storedReadings.Select(r => new
                {
                    time = r.Time,
                    reading = r.Reading
                }).ToList()
            };

            return Ok(response);
        }

        /// <summary>
        /// Retrieves meter readings for the specified smart meter.
        /// </summary>
        /// <param name="smartMeterId">The smart meter identifier.</param>
        /// <returns>A JSON object containing the smart meter ID and its electricity readings.</returns>
        [HttpGet("read/{smartMeterId}")]
        public IActionResult GetReading(string smartMeterId)
        {
            var readings = _meterReadingService.GetReadings(smartMeterId);
            if (readings == null || !readings.Any())
            {
                return NotFound($"No readings found for smart meter: {smartMeterId}");
            }

            var response = new
            {
                smartMeterId = smartMeterId,
                electricityReadings = readings.Select(r => new
                {
                    time = r.Time,
                    reading = r.Reading
                }).ToList()
            };

            return Ok(response);
        }

        /// <summary>
        /// Validates the MeterReadings payload.
        /// </summary>
        /// <param name="meterReadings">The payload to validate.</param>
        /// <returns>True if valid; otherwise, false.</returns>
        private bool IsMeterReadingsValid(SmartMeterReadings meterReadings)
        {
            return meterReadings != null &&
                   !string.IsNullOrWhiteSpace(meterReadings.SmartMeterId) &&
                   meterReadings.ElectricityReadings != null &&
                   meterReadings.ElectricityReadings.Any();
        }
    }
}
