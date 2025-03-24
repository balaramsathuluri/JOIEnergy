using JOIEnergy.API.Controllers;
using JOIEnergy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class PricePlanComparatorControllerTest
{
    private readonly Mock<IPricePlanService> _mockPricePlanService;
    private readonly Mock<IAccountService> _mockAccountService;
    private readonly PricePlanComparatorController _controller;

    private static readonly string PRICE_PLAN_1_ID = "test-supplier";
    private static readonly string PRICE_PLAN_2_ID = "best-supplier";
    private static readonly string PRICE_PLAN_3_ID = "second-best-supplier";
    private static readonly string SMART_METER_ID = "smart-meter-id";

    public PricePlanComparatorControllerTest()
    {
        _mockPricePlanService = new Mock<IPricePlanService>();
        _mockAccountService = new Mock<IAccountService>();

        _controller = new PricePlanComparatorController(_mockPricePlanService.Object, _mockAccountService.Object);
    }

    [Fact]
    public void GivenValidSmartMeterId_WhenCalculatingCost_ShouldReturnCorrectPricePlans()
    {
        var expectedCosts = new Dictionary<string, decimal>
        {
            { PRICE_PLAN_1_ID, 100m },
            { PRICE_PLAN_2_ID, 10m },
            { PRICE_PLAN_3_ID, 20m }
        };

        _mockPricePlanService.Setup(service => service.GetConsumptionCostOfElectricityReadingsForEachPricePlan(SMART_METER_ID))
            .Returns(expectedCosts);

        _mockAccountService.Setup(service => service.GetPricePlanIdForSmartMeterId(SMART_METER_ID))
            .Returns(PRICE_PLAN_1_ID);

        var result = _controller.CalculatedCostForEachPricePlan(SMART_METER_ID) as ObjectResult;
        var response = result.Value as Dictionary<string, object>;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(PRICE_PLAN_1_ID, response["pricePlanId"]);
        Assert.Equal(expectedCosts, response["pricePlanComparisons"]);

    }

    [Fact]
    public void GivenSmartMeterId_WhenRecommendingCheapestPlans_ShouldReturnOrderedList()
    {
        var expectedRecommendations = new List<KeyValuePair<string, decimal>>
    {
        new(PRICE_PLAN_2_ID, 38m),
        new(PRICE_PLAN_3_ID, 76m),
        new(PRICE_PLAN_1_ID, 380m)
    };

        _mockPricePlanService.Setup(service => service.GetConsumptionCostOfElectricityReadingsForEachPricePlan(SMART_METER_ID))
            .Returns(expectedRecommendations.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));

        var result = _controller.RecommendCheapestPricePlans(SMART_METER_ID, null) as ObjectResult;
        var response = ((IEnumerable<KeyValuePair<string, decimal>>)result.Value).ToList();

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(expectedRecommendations, response, new KeyValuePairComparer());
    }

    // Custom comparer for KeyValuePair<string, decimal>
    private class KeyValuePairComparer : IEqualityComparer<KeyValuePair<string, decimal>>
    {
        public bool Equals(KeyValuePair<string, decimal> x, KeyValuePair<string, decimal> y)
        {
            return x.Key == y.Key && x.Value == y.Value;
        }

        public int GetHashCode(KeyValuePair<string, decimal> obj)
        {
            return HashCode.Combine(obj.Key, obj.Value);
        }
    }


    [Fact]
    public void GivenUnknownSmartMeterId_WhenCalculatingCost_ShouldReturnNotFound()
    {
        _mockPricePlanService.Setup(service => service.GetConsumptionCostOfElectricityReadingsForEachPricePlan("unknown-meter"))
            .Returns(new Dictionary<string, decimal>());

        var result = _controller.CalculatedCostForEachPricePlan("unknown-meter") as NotFoundObjectResult;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public void GivenInvalidSmartMeterId_WhenCalculatingCost_ShouldReturnBadRequest()
    {
        var result = _controller.CalculatedCostForEachPricePlan("") as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public void GivenSmartMeterIdWithNoReadings_WhenRecommendingCheapestPlans_ShouldReturnEmptyList()
    {
        _mockPricePlanService.Setup(service => service.GetConsumptionCostOfElectricityReadingsForEachPricePlan(SMART_METER_ID))
            .Returns(new Dictionary<string, decimal>());

        var result = _controller.RecommendCheapestPricePlans(SMART_METER_ID, null) as ObjectResult;

        
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Smart Meter ID (smart-meter-id) not found", result.Value);
    }

    [Fact]
    public void GivenNullSmartMeterId_WhenCalculatingCost_ShouldReturnBadRequest()
    {
        var result = _controller.CalculatedCostForEachPricePlan(null) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }

    [Fact]
    public void GivenEmptySmartMeterId_WhenRecommendingCheapestPlans_ShouldReturnBadRequest()
    {
        var result = _controller.RecommendCheapestPricePlans("", null) as BadRequestObjectResult;

        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
    }
}
