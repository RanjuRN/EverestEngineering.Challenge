using Everest.CodingChallenge.CourierService.Model;
using Everest.CodingChallenge.CourierService.Services;
using Microsoft.Extensions.Configuration;


namespace Everest.CodingChallenge.CourierService.Tests.Services
{
    public class DeliveryPlannerServiceSchedulerTests
    {
        int vehicleCount;
        List<Package> packages;
        double maxSpeed;
        double maxCarryWeight;
        IConfiguration configuration;
        DeliveryPlannerServiceScheduler deliveryPlannerService;

        [SetUp]
        public void Setup()
        {
            vehicleCount = 2;
            maxSpeed = 70;
            maxCarryWeight = 200;
            packages = new List<Package>
            {
                new Package { Id = "PKG1", Weight = 50, DistanceToDestination = 30, ApplicableOfferCode = "OFR001" },
                new Package { Id = "PKG2", Weight = 75, DistanceToDestination = 125, ApplicableOfferCode = "OFR002" },
                new Package { Id = "PKG3", Weight = 175, DistanceToDestination = 100, ApplicableOfferCode = "OFR003" }
            };

            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("TestSettings.json").Build();

            deliveryPlannerService = new DeliveryPlannerServiceScheduler(vehicleCount, packages, maxSpeed, maxCarryWeight, configuration);
        }

        [Test]
        public void SchedulePackageDeliveries_ReturnsCorrectDeliveryTimes()
        {
            var result = deliveryPlannerService.SchedulePackageDeliveries(packages, vehicleCount, maxCarryWeight);

            Assert.IsNotNull(result);
            Assert.AreEqual(packages.Count, result.Count);
            Assert.IsTrue(result.ContainsKey("PKG1"));
            Assert.IsTrue(result["PKG1"] > 0);
        }

        [Test]
        public void GetSelectedPackageDeliveries_ReturnsPackageIds()
        {
            var selectedPackages = deliveryPlannerService.GetSelectedPackageDeliveries(packages, maxCarryWeight);

            Assert.IsNotNull(selectedPackages);
            Assert.IsTrue(selectedPackages.Count > 0);
            Assert.Contains("PKG1", selectedPackages);
        }

        [Test]
        public void GetPackagesDeliveriesByDeliveryTime_ReturnsDeliveryTimes()
        {
            
            var deliveryTimes = deliveryPlannerService.GetPackagesDeliveriesByDeliveryTime();

            Assert.IsNotNull(deliveryTimes);
            Assert.That(deliveryTimes.Count,Is.GreaterThan(0));
           
        }
    }
}