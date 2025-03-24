using JOIEnergy.Repository.Interfaces;
using JOIEnergy.Services.Implementations;
using JOIEnergy.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace JOIEnergy.Tests
{
    public class AccountServiceTest
    {
        private const string PRICE_PLAN_ID = "price-plan-id";
        private const string SMART_METER_ID = "smart-meter-id";

        private readonly Mock<ISmartMeterPricePlanRepository> _mockSmartMeterPricePlanRepository;
        private readonly Mock<ILogger<AccountService>> _mockLogger;
        private readonly IAccountService _accountService;

        public AccountServiceTest()
        {
            _mockSmartMeterPricePlanRepository = new Mock<ISmartMeterPricePlanRepository>();
            _mockLogger = new Mock<ILogger<AccountService>>();

            _mockSmartMeterPricePlanRepository.Setup(repo => repo.GetSmartMeterToPricePlanMappings())
                .Returns(new Dictionary<string, string>
                {
                { SMART_METER_ID, PRICE_PLAN_ID }
                });

            _accountService = new AccountService(_mockSmartMeterPricePlanRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public void GivenValidSmartMeterId_WhenRetrievingPricePlan_ShouldReturnCorrectPlanId()
        {
            var result = _accountService.GetPricePlanIdForSmartMeterId(SMART_METER_ID);
            Assert.Equal(PRICE_PLAN_ID, result);
        }

        [Fact]
        public void GivenUnknownSmartMeterId_WhenRetrievingPricePlan_ShouldReturnNull()
        {
            var result = _accountService.GetPricePlanIdForSmartMeterId("non-existent");
            Assert.Null(result);
        }

        [Fact]
        public void GivenNullSmartMeterId_WhenRetrievingPricePlan_ShouldReturnNull()
        {
            var result = _accountService.GetPricePlanIdForSmartMeterId(null);
            Assert.Null(result);
        }

        [Fact]
        public void GivenEmptySmartMeterId_WhenRetrievingPricePlan_ShouldReturnNull()
        {
            var result = _accountService.GetPricePlanIdForSmartMeterId("");
            Assert.Null(result);
        }
    }
}