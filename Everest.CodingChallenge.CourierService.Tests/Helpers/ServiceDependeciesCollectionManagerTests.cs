using Everest.CodingChallenge.CourierService.Helpers;
using Everest.CodingChallenge.CourierService.Interfaces;
using Everest.CodingChallenge.CourierService.Model;
using Everest.CodingChallenge.CourierService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Everest.CodingChallenge.CourierService.Tests.Helpers;

public class ServiceDependeciesCollectionManagerTests
{
    
    IConfiguration configuration;
    [SetUp]
    public void Setup()
    {
        configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("TestSettings.json").Build();

    }

    [Test]
    public void RegisterDependendencies_Success()
    {
        var mockServiceCollection = new Mock<IServiceCollection>();
        IOfferCodeSeviceOperations offerCodeSeviceOperations = new OfferCodeService(configuration);                
        var serviceCollection = mockServiceCollection.Object;
        serviceCollection.AddTransient<IOfferCodeSeviceOperations, OfferCodeService>();

        var dependencies = ServiceDependeciesCollectionManager.RegisterDependendencies(serviceCollection, configuration);
        Assert.IsNotNull(dependencies);
    }
}
