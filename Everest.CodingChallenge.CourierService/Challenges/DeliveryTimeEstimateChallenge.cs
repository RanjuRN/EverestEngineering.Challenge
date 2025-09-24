using Everest.CodingChallenge.CourierService.Interfaces;
using Everest.CodingChallenge.CourierService.Model;
using Everest.CodingChallenge.CourierService.Services;
using Microsoft.Extensions.Configuration;

namespace Everest.CodingChallenge.CourierService.Challenges
{
    /// <summary>
    /// Class to control the flow of associated with the challenege of estimates for determining the delivery time
    /// </summary>
    public class DeliveryTimeEstimateChallenge : IDeliveryTimeEstimateChallengeOperations
    {
        IOfferCodeSeviceOperations offerCodeService;
        IDeliveryPlannerServiceOperations deliveryPlannerServiceOperations;
        IConfiguration configuration;
        List<Package> packages;
        public DeliveryTimeEstimateChallenge(IConfiguration configuration, IOfferCodeSeviceOperations offerCodeSevice, List<Package> packages = null,IDeliveryPlannerServiceOperations deliveryPlannerServiceOperations = null)
        {
            this.configuration = configuration;
            // Pre populate Offer Codes
            offerCodeService = offerCodeSevice;
            this.packages = packages ?? new List<Package>();
            if (deliveryPlannerServiceOperations != null)
            {
                this.deliveryPlannerServiceOperations = deliveryPlannerServiceOperations;
            }
        }
       
        public bool ChallengeSolved() 
        { 
            var deliveriesByTime = deliveryPlannerServiceOperations.GetPackagesDeliveriesByDeliveryTime().Count;
            return deliveriesByTime > 0;        
        }

        public void StartChallenge()
        {
            Console.WriteLine("Enter Packages info in the format(first) - base_delivery_cost no_of_packges then on each separate line:- pkg_id1 pkg_weight1_in_kg distance1_in_km offer_code1");
            Console.WriteLine("After all the package info entered input info in the format- no_of_vehicles max_speed max_carriable_weight");
            string input = Console.ReadLine();
            string[] inputs = input.Split(' ');
            double baseDeliveryCost = Convert.ToDouble(inputs[0]);
            int noOfPackages = Convert.ToInt32(inputs[1]);
                        
            for (int i = 0; i < noOfPackages; i++)
            {
                string packageInput = Console.ReadLine();
                string[] packageInputs = packageInput.Split(' ');
                Package package = new Package()
                {
                    Id = packageInputs[0],
                    Weight = Convert.ToDouble(packageInputs[1]),
                    DistanceToDestination = Convert.ToDouble(packageInputs[2]),
                    ApplicableOfferCode = packageInputs.Length > 3 ? packageInputs[3] : null
                };
                packages.Add(package);
            }

            string vehicleParamInputs = Console.ReadLine();
            string[] vehicleParams = vehicleParamInputs.Split(' ');
            int noOfVehicles = Convert.ToInt32(vehicleParams[0]);
            double maxSpeed = Convert.ToDouble(vehicleParams[1]);
            double maxCarryWeight = Convert.ToDouble(vehicleParams[2]);

            deliveryPlannerServiceOperations = new DeliveryPlannerServiceScheduler(noOfVehicles, packages, maxSpeed, maxCarryWeight, configuration);
            var packagesbyDeliveryTime = deliveryPlannerServiceOperations.GetPackagesDeliveriesByDeliveryTime();

            Console.WriteLine("Output in the format(spaces in between):- packageid discount total_cost");
            foreach (var package in packages)
            {
                Business.DeliveryCostCalculator deliveryCostCalculator = new Business.DeliveryCostCalculator(baseDeliveryCost, package);
                double deliveryCost = deliveryCostCalculator.GetCalculatedDeliveryCost();
                Business.OfferDiscountCalculator offerDiscountCalculator = new Business.OfferDiscountCalculator(package, offerCodeService.GetAllOfferCodes(), deliveryCost);
                double discount = offerDiscountCalculator.GetCalculatedOfferDiscount();
                double totalCost = deliveryCostCalculator.GetTotalDeliveryCost(discount);
                double deliveryTime = packagesbyDeliveryTime[package.Id];

                Console.WriteLine($"{package.Id} {Math.Truncate(discount)} {totalCost} {deliveryTime.ToString("F2")}"); //delivery time 2 digits after decimal
            }

        }
    }
}
