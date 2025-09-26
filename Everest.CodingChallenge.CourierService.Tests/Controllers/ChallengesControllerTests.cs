using Everest.CodingChallenge.CourierService.Business;
using Everest.CodingChallenge.CourierService.Challenges;
using Everest.CodingChallenge.CourierService.Controller;
using Everest.CodingChallenge.CourierService.Interfaces;
using Everest.CodingChallenge.CourierService.Model;
using Everest.CodingChallenge.CourierService.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Everest.CodingChallenge.CourierService.Tests;

public class ChallengesControllerTests
{
    private ChallengesController controller;
    IConfiguration configuration;
    IDeliveryCostEstimateChallengeOperations challenge1;
    IDeliveryTimeEstimateChallengeOperations challenge2;
    List<Package> packages;
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
        
        packages = new List<Package>
        {
            new Package { Id = "PKG1", Weight = 50, DistanceToDestination = 30, ApplicableOfferCode = "OFR001" },
            new Package { Id = "PKG2", Weight = 75, DistanceToDestination = 125, ApplicableOfferCode = "OFR002" },
            new Package { Id = "PKG3", Weight = 175, DistanceToDestination = 100, ApplicableOfferCode = "OFR003" }
        };
                                                                                                                                                                                                                                                                        
    }

    [Test]
    public void GetCostEstimateChallenge_ReturnsOkResult()
    {
        var offerCodeServiceMock = new Mock<IOfferCodeSeviceOperations>();
        offerCodeService = offerCodeServiceMock.Object;
        offerCodeServiceMock.Setup(s => s.GetAllOfferCodes()).Returns(new List<Model.OfferCodes>
        {
            new Model.OfferCodes { Code = "OFR001", DiscountPercentage = 10, MinWeight = 70, MaxWeight = 200, MinDistance = 0, MaxDistance = 199 },
            new Model.OfferCodes { Code = "OFR002", DiscountPercentage = 7, MinWeight = 100, MaxWeight = 250, MinDistance = 50, MaxDistance = 150 },
            new Model.OfferCodes { Code = "OFR003", DiscountPercentage = 5, MinWeight = 10, MaxWeight = 150, MinDistance = 50, MaxDistance = 250 }
        });

       
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

        Mock<IDeliveryCostEstimateChallengeOperations> challenge1Mock = new Mock<IDeliveryCostEstimateChallengeOperations>();
        Mock<IDeliveryTimeEstimateChallengeOperations> challenge2Mock = new Mock<IDeliveryTimeEstimateChallengeOperations>();
        challenge1 = challenge1Mock.Object;
        var mockConsoleOperations = new Mock<IIOServiceOperations>();
        var inputSequence = new Queue<string>(new[]
        {
            "100 2",                // baseDeliveryCost and noOfPackages
            "PKG1 5 5 OFR001",      // Package 1
            "PKG2 10 100 OFR002"    // Package 2
        });
        mockConsoleOperations.Setup(x => x.ReadLine()).Returns(() => inputSequence.Dequeue());
        
        //var configuration = new Mock<IConfiguration>().Object;
        var challenge = new DeliveryCostEstimateChallenge(configuration,packages, consoleOperations: mockConsoleOperations.Object);
        
        // Act
        challenge.StartChallenge();

        // Assert
        mockConsoleOperations.Verify(x => x.ReadLine(), Times.Exactly(3));

    }

    [Test]

    public void Orchestrate_Challenges_CallsStartChallenge_OnBothChallenges()
    {
        // Arrange
        var challenge1Mock = new Mock<IDeliveryCostEstimateChallengeOperations>();
        var challenge2Mock = new Mock<IDeliveryTimeEstimateChallengeOperations>();
        controller = new ChallengesController(configuration, challenge1Mock.Object, challenge2Mock.Object);
        // Act
        controller.OrchestrateChallenges();
        // Assert
        challenge1Mock.Verify(c => c.StartChallenge(), Times.Once);
        challenge2Mock.Verify(c => c.StartChallenge(), Times.Once);
    }

}

