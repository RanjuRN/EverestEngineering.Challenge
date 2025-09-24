using Everest.CodingChallenge.CourierService.Challenges;
using Everest.CodingChallenge.CourierService.Controller;
using Everest.CodingChallenge.CourierService.Interfaces;
using Everest.CodingChallenge.CourierService.Model;
using Everest.CodingChallenge.CourierService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.CodingChallenge.CourierService.Helpers
{
    public static class ServiceDependeciesCollectionManager
    {

        public static IServiceCollection  RegisterDependendencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IOfferCodeSeviceOperations, OfferCodeService>();                   
            services.Configure<DeliveryPlannerServiceOptions>(configuration.GetSection(DeliveryPlannerServiceOptions.DeliveryPlannerServiceConfigHeader));
            services.AddTransient<IDeliveryPlannerServiceOperations, DeliveryPlannerServiceScheduler>();
            services.AddScoped<IDeliveryCostEstimateChallengeOperations, DeliveryCostEstimateChallenge>();
            services.AddScoped<IDeliveryTimeEstimateChallengeOperations, DeliveryTimeEstimateChallenge>();
            services.AddSingleton<ChallengesController>();

            return services;
        }
        
    }
}
