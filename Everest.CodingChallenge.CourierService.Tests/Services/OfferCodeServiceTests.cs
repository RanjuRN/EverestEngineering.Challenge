using Everest.CodingChallenge.CourierService.Model;
using Everest.CodingChallenge.CourierService.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;

namespace Everest.CodingChallenge.CourierService.Tests;

public class OfferCodeServiceTests
{
    OfferCodeService offerCodeService;
    OfferCodes offerCodes;
    IConfiguration configuration;

    [SetUp]
    public void Setup()
    {
        /*configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("TestSettings.json").Build();*/

        string  configString = "'Model': { 'OfferCodes': { 'ConfigFilePath': 'TestData/OfferCodes.json''}";

        //IEnumerable<KeyValuePair> keyValuePairConfig = JsonSerializer. 
        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("TestSettings.json").Build(); 

        //var mockConfiguration = new Mock<IConfiguration>(configuration);
        //mockConfiguration.Setup(config => config["Model:OfferCodes:ConfigFilePath"]).Returns("TestData/OfferCodes.json");
        //mockConfiguration.SetupGet(config => configconfigString);
        offerCodeService = new OfferCodeService(configuration);


        
    }

    [Test]
    public void GetAllOfferCodesNotNullOrEmpty_Success()
    {
        var offerCodes=offerCodeService.GetAllOfferCodes();
        string ofrCodeSerialized = JsonConvert.SerializeObject(offerCodes,Formatting.Indented);
        Assert.True(!string.IsNullOrEmpty(ofrCodeSerialized));
    }

    [Test]
    public void AddOfferCode_Success()
    {
        offerCodes = new OfferCodes()
        {
            Code = "TEST0001",
            DiscountPercentage = 15,
            MinWeight = 50,
            MaxWeight = 300,
            MinDistance = 10,
            MaxDistance = 200
        };
        offerCodeService.AddOfferCode(offerCodes);
        var offerCodesList = offerCodeService.GetAllOfferCodes();
        Assert.IsTrue(offerCodesList.Any(o => o.Code == "TEST0001"));
    }

}
