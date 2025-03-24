using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Domain.Models;
using JOIEnergy.Repository.Interfaces;
using JOIEnergy.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace JOIEnergy.Services.Implementations
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly ILogger<MeterReadingService> _logger;
        private readonly IMeterReadingRepository _meterReadingRepository;

        public MeterReadingService(IMeterReadingRepository meterReadingRepository, ILogger<MeterReadingService> logger)
        {
            _meterReadingRepository = meterReadingRepository;
            _logger = logger;
        }

        public List<ElectricityReading> GetReadings(string smartMeterId)
        {
            _logger.LogInformation("Fetching readings for SmartMeterId: {SmartMeterId}", smartMeterId);
            return _meterReadingRepository.GetReadings(smartMeterId);
        }

        public void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings)
        {
            _logger.LogInformation("Storing {Count} readings for SmartMeterId: {SmartMeterId}", electricityReadings.Count, smartMeterId);
            _meterReadingRepository.StoreReadings(smartMeterId, electricityReadings);
        }
    }
}