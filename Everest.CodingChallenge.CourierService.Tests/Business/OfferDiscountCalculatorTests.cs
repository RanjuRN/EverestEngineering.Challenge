using Everest.CodingChallenge.CourierService.Business;
using Everest.CodingChallenge.CourierService.Model;

namespace Everest.CodingChallenge.CourierService.Tests;

public class OfferDiscountCalculatorTests
{
    OfferDiscountCalculator calculator;
    Package package;
    double discount;
    double deliveryCost;
    List<OfferCodes> offerCodes;

    [SetUp]
    public void Setup()
    {
        offerCodes = new List<OfferCodes>
        {
            new OfferCodes { Code = "OFR001", MinWeight = 70, MaxWeight = 200, MinDistance = 0, MaxDistance = 199, DiscountPercentage = 10 },
            new OfferCodes { Code = "OFR002", MinWeight = 100, MaxWeight = 250, MinDistance = 50, MaxDistance = 150, DiscountPercentage = 7 },
            new OfferCodes { Code = "OFR003", MinWeight = 10, MaxWeight = 150, MinDistance = 50, MaxDistance = 250, DiscountPercentage = 5 }
        };
        deliveryCost = 500;
        discount = 10;
        package = new Package
        {
            Weight = 75,
            DistanceToDestination = 30,
            ApplicableOfferCode = "OFR001"
        };
       
        calculator = new OfferDiscountCalculator(package,offerCodes,deliveryCost);
    }

    [Test]
    public void CalculateDiscount_ValidOfferCode_ReturnsDiscount_Success()
    {
        
        // Act
        var discount = calculator.GetCalculatedOfferDiscount();

        // Assert
        Assert.That(discount, Is.GreaterThan(0));
    }

    
}
