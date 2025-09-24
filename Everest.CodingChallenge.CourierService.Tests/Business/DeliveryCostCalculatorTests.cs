using Everest.CodingChallenge.CourierService.Business;
using Everest.CodingChallenge.CourierService.Model;
using Everest.CodingChallenge.CourierService.Services;
using Microsoft.Extensions.Configuration;

namespace Everest.CodingChallenge.CourierService.Tests.Business;

public class DeliveryCostCalculatorTests
{

    Package package;
    DeliveryCostCalculator deliveryCostCalculator;

    [SetUp]
    public void Setup()
    {
        package = new Package
        {
            Id = "PKG1",
            Weight = 50,
            DistanceToDestination = 30,
            ApplicableOfferCode = "OFR001"
        };

        deliveryCostCalculator = new DeliveryCostCalculator(100, package);

    }

    [Test]
    public void GetCalculatedDeliveryCost_ReturnsSuccess()
    {
        var cost = deliveryCostCalculator.GetCalculatedDeliveryCost();
        Assert.That(Convert.ToDouble(100 + 50 * 10 + 30 * 5), Is.EqualTo(cost));
    }

    [Test]
    public void GetTotalDeliveryCost_ReturnsSuccess()
    {
        var totalCost = deliveryCostCalculator.GetTotalDeliveryCost(50);
        Assert.That(Convert.ToDouble(100 + 50 * 10 + 30 * 5 - 50), Is.EqualTo(totalCost));
    }
}
