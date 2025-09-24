using Everest.CodingChallenge.CourierService.Business;
using Everest.CodingChallenge.CourierService.Challenges;
using Everest.CodingChallenge.CourierService.Interfaces;
using Everest.CodingChallenge.CourierService.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Everest.CodingChallenge.CourierService.Tests.Challenges;

public class DeliveryCostEstimateChallengeTests
{
     DeliveryCostEstimateChallenge challenge;
     IConfiguration configuration;
     IOfferCodeSeviceOperations offerCodeService;
     IDeliveryPlannerServiceOperations deliveryPlannerServiceOperations;
    IOfferDiscountCalculatorOperations offerDiscountCalculatorOperations;
    IDeliveryCostCalculatorOperations deliveryCostCalculator;

    [SetUp]
    public void Setup()
    {
        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("TestSettings.json").Build();
      
        
    }

    [Test]
    public void CalculateCost_WithValidInputs_ReturnsExpectedCost()
    {
        // Arrange
        var offerCodeServiceMock = new Mock<IOfferCodeSeviceOperations>();
        offerCodeService = offerCodeServiceMock.Object;
        offerCodeServiceMock.Setup(s => s.GetAllOfferCodes()).Returns(new List<Model.OfferCodes>
        {
            new Model.OfferCodes { Code = "OFR001", DiscountPercentage = 10, MinWeight = 70, MaxWeight = 200, MinDistance = 0, MaxDistance = 199 },
            new Model.OfferCodes { Code = "OFR002", DiscountPercentage = 7, MinWeight = 100, MaxWeight = 250, MinDistance = 50, MaxDistance = 150 },
            new Model.OfferCodes { Code = "OFR003", DiscountPercentage = 5, MinWeight = 10, MaxWeight = 150, MinDistance = 50, MaxDistance = 250 }
        });

        List<Model.Package> packages = new List<Model.Package>
        {
            new Model.Package { Id = "PKG1", Weight = 50, DistanceToDestination = 30, ApplicableOfferCode = "OFR001" },
            new Model.Package { Id = "PKG2", Weight = 75, DistanceToDestination = 125, ApplicableOfferCode = "OFR002" },
            new Model.Package { Id = "PKG3", Weight = 175, DistanceToDestination = 100, ApplicableOfferCode = "OFR003" }
        };
        var deliveryPlannerServiceOperationsMock = new Mock<IDeliveryPlannerServiceOperations>();
        deliveryPlannerServiceOperations = deliveryPlannerServiceOperationsMock.Object;
        deliveryPlannerServiceOperationsMock.Setup(d => d.SchedulePackageDeliveries(It.IsAny<List<Model.Package>>(), It.IsAny<int>(), It.IsAny<double>()))
            .Returns(new Dictionary<string, double>
            {
                { "PKG1", 3.98 },
                { "PKG2", 1.78 },
                { "PKG3", 1.42 }
            });

        var offerDiscountCalculatorMock = new Mock<IOfferDiscountCalculatorOperations>();
        offerDiscountCalculatorOperations = offerDiscountCalculatorMock.Object;
        offerDiscountCalculatorMock.Setup(o => o.GetCalculatedOfferDiscount());

        offerDiscountCalculatorOperations = new OfferDiscountCalculator(new Model.Package { Id = "PKG1", Weight = 50, DistanceToDestination = 30, ApplicableOfferCode = "OFR001" }, offerCodeServiceMock.Object.GetAllOfferCodes(), 100);

        deliveryCostCalculator = new DeliveryCostCalculator(100, new Model.Package { Id = "PKG1", Weight = 50, DistanceToDestination = 30, ApplicableOfferCode = "OFR001" });

        // Act
        challenge = new DeliveryCostEstimateChallenge(configuration, offerDiscountCalculatorOperations, offerCodeService, deliveryCostCalculator, packages);
        
        // Assert
        Assert.IsTrue(challenge.ChallengeSolved());
        


    }

    
}
