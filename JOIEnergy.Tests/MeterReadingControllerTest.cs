using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.API.Controllers;
using JOIEnergy.Domain.Models;
using JOIEnergy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace JOIEnergy.Tests
{
    public class MeterReadingControllerTests
    {
        private readonly Mock<IMeterReadingService> _meterReadingServiceMock;
        private readonly Mock<ILogger<MeterReadingController>> _loggerMock;
        private readonly MeterReadingController _controller;

        public MeterReadingControllerTests()
        {
            _meterReadingServiceMock = new Mock<IMeterReadingService>();
            _loggerMock = new Mock<ILogger<MeterReadingController>>();
            _controller = new MeterReadingController(_meterReadingServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void StoreReading_ReturnsBadRequest_WhenInvalidMeterReadings()
        {
            // Arrange
            var invalidReading = new SmartMeterReadings { SmartMeterId = "", ElectricityReadings = null };

            // Act
            var result = _controller.StoreReading(invalidReading);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void StoreReading_ReturnsOk_WhenValidMeterReadings()
        {
            // Arrange
            var validReading = new SmartMeterReadings
            {
                SmartMeterId = "12345",
                ElectricityReadings = new List<ElectricityReading> { new ElectricityReading() }
            };

            _meterReadingServiceMock
                .Setup(s => s.GetReadings(It.IsAny<string>()))
                .Returns(validReading.ElectricityReadings);

            // Act
            var result = _controller.StoreReading(validReading);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
