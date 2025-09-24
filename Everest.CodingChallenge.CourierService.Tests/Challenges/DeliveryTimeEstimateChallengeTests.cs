using Everest.CodingChallenge.CourierService.Challenges;
using Everest.CodingChallenge.CourierService.Interfaces;
using Everest.CodingChallenge.CourierService.Model;
using Everest.CodingChallenge.CourierService.Services;
using Microsoft.Extensions.Configuration;

namespace Everest.CodingChallenge.CourierService.Tests.Challenges;

public class DeliveryTimeEstimateChallengeTests
{
    IOfferCodeSeviceOperations offerCodeService;
    IDeliveryPlannerServiceOperations deliveryPlannerServiceOperations;
    IConfiguration configuration;
    List<Package> packages;
    DeliveryTimeEstimateChallenge challenge;
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
    /*

    [Test]
    public void EstimateDeliveryTime_ReturnsExpectedHours_ForLongDistance()
    {
        // Arrange
        var challenge = new DeliveryTimeEstimateChallenge();
        double distanceKm = 100.0;
        string courierType = "Van";

        // Act
        double estimatedHours = challenge.EstimateDeliveryTime(distanceKm, courierType);

        // Assert
        Assert.AreEqual(2.0, estimatedHours, 0.01);
    }
    */
}
