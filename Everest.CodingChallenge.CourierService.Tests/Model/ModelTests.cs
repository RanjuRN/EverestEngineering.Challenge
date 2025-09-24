using Everest.CodingChallenge.CourierService.Model;

namespace Everest.CodingChallenge.CourierService.Tests;

public class ModelTests
{

    DeliveryPlannerServiceOptions deliveryPlannerServiceOptions;
    OfferCodes offerCodes;
    Package package;
    Vehicle vehicle;
    VehicleByAvailabilityTime vehicleByAvailabilityTime;
    VehiclePackageAssignment vehiclePackageAssignment;

    [SetUp]
    public void Setup()
    {
        // Bulk instiation of all model classes
        offerCodes = new OfferCodes();
        deliveryPlannerServiceOptions = new DeliveryPlannerServiceOptions();       
        package = new Package();
        vehicle = new Vehicle();
        vehiclePackageAssignment = new VehiclePackageAssignment();
        
    }

    [Test]
    public void CheckOfferValuesSet_Success()
    {
        offerCodes.Code = "OFFR001";
        offerCodes.MaxWeight = 100;
        offerCodes.MinWeight = 20;
        offerCodes.DiscountPercentage = 7;
        offerCodes.MaxDistance = 100;
        offerCodes.MinDistance = 10;

        Assert.Multiple(() => {
            Assert.AreEqual("OFFR001", offerCodes.Code);
            Assert.AreEqual(100, offerCodes.MaxWeight);
            Assert.AreEqual(20, offerCodes.MinWeight);
            Assert.AreEqual(7, offerCodes.DiscountPercentage);
            Assert.AreEqual(100, offerCodes.MaxDistance);
            Assert.AreEqual(10, offerCodes.MinDistance);

            });

    }
    [Test]
    public void CheckDeliveryPlannerSet_Success()
    {
        deliveryPlannerServiceOptions.VehicleCount = 1;
        deliveryPlannerServiceOptions.MaxCarryWeight = 100;
        deliveryPlannerServiceOptions.MaxSpeed = 100;

        Assert.Multiple(() =>
        {
            Assert.AreEqual(1, deliveryPlannerServiceOptions.VehicleCount);
            Assert.AreEqual(100, deliveryPlannerServiceOptions.MaxCarryWeight);
            Assert.AreEqual(100, deliveryPlannerServiceOptions.MaxSpeed);
        });
    }

    [Test]
    public void CheckPackageValuesSet_Success()
    {
        package = new Package();
        package.Weight = 100;
        package.ApplicableOfferCode = "OFFR001";
        package.DistanceToDestination = 100;
        package.Id = "v1";

        Assert.Multiple(() => { 

            Assert.AreEqual(100, package.Weight);
            Assert.AreEqual("OFFR001", package.ApplicableOfferCode);
            Assert.AreEqual(100,package.DistanceToDestination);
            Assert.AreEqual("v1", package.Id);
        
        });

    }

    [Test]
    public void CheckVehicleValuesSet_Success()
    {
        vehicle.MaxCarryWeight = 1; 
        vehicle.MaxSpeed = 100; 
        Assert.Multiple(() =>
        {
            Assert.AreEqual(1,vehicle.MaxCarryWeight);
            Assert.IsTrue(vehicle.MaxSpeed > 0);
        });
    }

    [Test]
    public void CheckVehiclePackageAssignmentValuesSet_Success()
    {
        vehiclePackageAssignment.currentVehicleWeight = 100;
        vehiclePackageAssignment.vehicleId = 1;

        Assert.Multiple(() =>
        {
            Assert.AreEqual(100, vehiclePackageAssignment.currentVehicleWeight);
            Assert.AreEqual(1, vehiclePackageAssignment.vehicleId);
        });
        
    }
}
