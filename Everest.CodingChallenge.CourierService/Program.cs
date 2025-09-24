using Everest.CodingChallenge.CourierService.Controller;
using Everest.CodingChallenge.CourierService.Helpers;
using Everest.CodingChallenge.CourierService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static partial class Program
{
    static void Main(string[] args)
    {

        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddCommandLine(args);

        builder.Services.RegisterDependendencies(builder.Configuration);

        var challengesController = builder.Services.BuildServiceProvider().GetRequiredService<ChallengesController>();
        challengesController.OrchestrateChallenges();
        builder.Build().Run();


    }
}


