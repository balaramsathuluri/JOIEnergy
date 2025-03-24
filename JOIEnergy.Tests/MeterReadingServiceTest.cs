using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Domain.Models;
using JOIEnergy.Repository.Interfaces;
using JOIEnergy.Services.Implementations;
using Moq;

namespace JOIEnergy.Tests
{
    public class MeterReadingServiceTest
    {        

        private readonly Mock<IMeterReadingRepository> _mockMeterReadingRepository;
        private readonly MeterReadingService _meterReadingService;

        public MeterReadingServiceTest()
        {           

            _mockMeterReadingRepository = new Mock<IMeterReadingRepository>();

            _mockMeterReadingRepository
                .Setup(repo => repo.GetReadings("smart-meter-1"))
                .Returns(new List<ElectricityReading>
                {
                new ElectricityReading { Time = DateTime.UtcNow.AddHours(-1), Reading = 50m },
                new ElectricityReading { Time = DateTime.UtcNow, Reading = 60m }
                });

            _meterReadingService = new MeterReadingService(_mockMeterReadingRepository.Object);
        }

        [Fact]
        public void GivenMeterIdThatDoesNotExistShouldReturnNull()
        {
            _mockMeterReadingRepository.Setup(repo => repo.GetReadings("unknown-meter")).Returns(new List<ElectricityReading>());

            var readings = _meterReadingService.GetReadings("unknown-meter");
            Assert.Empty(readings);
        }

        [Fact]
        public void GivenMeterReadingThatExistsShouldReturnMeterReadings()
        {
            var readings = _meterReadingService.GetReadings("smart-meter-1");
            Assert.NotEmpty(readings);
            Assert.Equal(2, readings.Count);
        }

        [Fact]
        public void GivenNullMeterId_WhenFetchingReadings_ShouldReturnEmptyList()
        {
            var readings = _meterReadingService.GetReadings(null);
            Assert.Null(readings);
        }

        [Fact]
        public void GivenSmartMeterIdWithNoReadings_WhenFetchingReadings_ShouldReturnEmptyList()
        {
            _mockMeterReadingRepository.Setup(repo => repo.GetReadings("empty-meter"))
                .Returns(new List<ElectricityReading>());

            var readings = _meterReadingService.GetReadings("empty-meter");
            Assert.Empty(readings);
        }

    }
}
