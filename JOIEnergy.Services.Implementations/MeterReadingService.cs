using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Domain.Models;
using JOIEnergy.Repository.Interfaces;
using JOIEnergy.Services.Interfaces;

namespace JOIEnergy.Services.Implementations
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IMeterReadingRepository _meterReadingRepository;

        public MeterReadingService(IMeterReadingRepository meterReadingRepository)
        {
            _meterReadingRepository = meterReadingRepository;
        }

        public List<ElectricityReading> GetReadings(string smartMeterId)
        {
            return _meterReadingRepository.GetReadings(smartMeterId);
        }

        public void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings)
        {
            _meterReadingRepository.StoreReadings(smartMeterId, electricityReadings);
        }
    }
}
