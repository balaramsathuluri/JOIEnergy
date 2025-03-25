using JOIEnergy.Domain.Models;
using JOIEnergy.Repository.Interfaces;
using JOIEnergy.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace JOIEnergy.Services.Implementations
{
    public class MeterReadingService : IMeterReadingService
    {
        #region Private Members

        private readonly ILogger<MeterReadingService> _logger;
        private readonly IMeterReadingRepository _meterReadingRepository;

        #endregion

        #region Constructor
        public MeterReadingService(IMeterReadingRepository meterReadingRepository, ILogger<MeterReadingService> logger)
        {
            _meterReadingRepository = meterReadingRepository;
            _logger = logger;
        }
        #endregion

        #region Public Methods
        public List<ElectricityReading> GetReadings(string smartMeterId)
        {
            var readings = _meterReadingRepository.GetReadings(smartMeterId) ?? new List<ElectricityReading>();  // ✅ Ensure it's never null

            if (!readings.Any())
            {
                _logger.LogWarning("No readings found for SmartMeterId: {SmartMeterId}", smartMeterId);
            }

            return readings;
        }



        public void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings)
        {
            _logger.LogInformation("Storing {Count} readings for SmartMeterId: {SmartMeterId}", electricityReadings.Count, smartMeterId);
            _meterReadingRepository.StoreReadings(smartMeterId, electricityReadings);
        }
        #endregion
    }
}