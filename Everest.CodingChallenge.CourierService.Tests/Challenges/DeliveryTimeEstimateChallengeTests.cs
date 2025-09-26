using Everest.CodingChallenge.CourierService.Challenges;
using Everest.CodingChallenge.CourierService.Interfaces;
using Everest.CodingChallenge.CourierService.Model;
using Everest.CodingChallenge.CourierService.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Everest.CodingChallenge.CourierService.Tests.Challenges;

public class DeliveryTimeEstimateChallengeTests
{
    IOfferCodeSeviceOperations offerCodeService;
    IDeliveryPlannerServiceOperations deliveryPlannerServiceOperations;
    IConfiguration configuration;
    List<Package> packages;
    DeliveryTimeEstimateChallenge challenge;
    IIOServiceOperations consoleOperations;

    [SetUp]
    public void Setup()
    {
        configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("TestSettings.json").Build();
        
        offerCodeService = new OfferCodeService(configuration);
        packages = new List<Package>();
        deliveryPlannerServiceOperations = new DeliveryPlannerServiceScheduler(2, packages, 70, 200, configuration);

    }

    [Test]
    public void EstimateDeliveryTime_ReturnsExpectedHours_ForShortDistance()
    {
        // Arrange
        packages.Add(new Package { Id = "PKG1", Weight = 50, DistanceToDestination = 30, ApplicableOfferCode = "OFR001" });
        challenge = new DeliveryTimeEstimateChallenge(configuration, offerCodeService, packages, deliveryPlannerServiceOperations);
        // Act

        // Assert
        Assert.IsTrue(challenge.ChallengeSolved());
    }

    [Test]
    public void StartChallenge_Calls_Correctly()
    {
        // Arrange
        packages.Add(new Package { Id = "PKG1", Weight = 50, DistanceToDestination = 30, ApplicableOfferCode = "OFR001" });
        packages.Add(new Package { Id = "PKG2", Weight = 75, DistanceToDestination = 125, ApplicableOfferCode = "OFR002" });
        packages.Add(new Package { Id = "PKG3", Weight = 175, DistanceToDestination = 100, ApplicableOfferCode = "OFR003" });
        var challengeMock = new Mock<IDeliveryCostEstimateChallengeOperations>();
       
        // Act
        challengeMock.Object.StartChallenge();
        
        // Assert
        challengeMock.Verify(x => x.StartChallenge(), Times.Once);
    }


    [Test]

    public void GetInputPackageParameters_Returns_AsExpected()
    {
        // Arrange
        var mockConsoleOperations = new Mock<IIOServiceOperations>();
        mockConsoleOperations.Setup(x => x.ReadLine()).Returns("100 3");        
        
        packages.Add(new Package { Id = "PKG1", Weight = 50, DistanceToDestination = 30, ApplicableOfferCode = "OFR001" });
        challenge = new DeliveryTimeEstimateChallenge(configuration, offerCodeService, packages, deliveryPlannerServiceOperations, consoleOperations: mockConsoleOperations.Object);
        double baseDeliveryCost;
        int noOfPackages;
        
        // Act       
        challenge.GetInputPackageParameters(out baseDeliveryCost, out noOfPackages);

        // Assert
        Assert.AreEqual(100, baseDeliveryCost);
        Assert.AreEqual(3, noOfPackages);
    }

    [Test]
    public void ReadPackages_Returns_CorrectNumberOfPackages()
    {
        // Arrange
        var mockConsoleOperations = new Mock<IIOServiceOperations>();
        mockConsoleOperations.SetupSequence(x => x.ReadLine())
            .Returns("PKG1 50 30 OFR001")
            .Returns("PKG2 75 125 OFR002")
            .Returns("PKG3 175 100 OFR003");
        challenge = new DeliveryTimeEstimateChallenge(configuration, offerCodeService, packages, deliveryPlannerServiceOperations, consoleOperations: mockConsoleOperations.Object);
        
        // Act
        var result = challenge.ReadPackages(3, mockConsoleOperations.Object);
        // Assert
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("PKG1", result[0].Id);
        Assert.AreEqual(50, result[0].Weight);
        Assert.AreEqual(30, result[0].DistanceToDestination);
        Assert.AreEqual("OFR001", result[0].ApplicableOfferCode);
    }

    [Test]

    public void ReadVehicleParameters_Returns_CorrectValues()
    {
        // Arrange
        var mockConsoleOperations = new Mock<IIOServiceOperations>();
        mockConsoleOperations.Setup(x => x.ReadLine()).Returns("2 70 200");
        challenge = new DeliveryTimeEstimateChallenge(configuration, offerCodeService, packages, deliveryPlannerServiceOperations, consoleOperations: mockConsoleOperations.Object);
        int noOfVehicles;
        double maxSpeed;
        double maxCarryWeight;
        // Act
        challenge.ReadVehicleParameters(mockConsoleOperations.Object, out noOfVehicles, out maxSpeed, out maxCarryWeight);
        // Assert
        Assert.AreEqual(2, noOfVehicles);
        Assert.AreEqual(70, maxSpeed);
        Assert.AreEqual(200, maxCarryWeight);
    }
}
