using Everest.CodingChallenge.CourierService.Business;
using Everest.CodingChallenge.CourierService.Interfaces;
using Everest.CodingChallenge.CourierService.Model;
using Everest.CodingChallenge.CourierService.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.CodingChallenge.CourierService.Challenges
{
    /// <summary>
    /// Class to control the flow of associated with the challenege of estimates for determining the delivery costs
    /// </summary>
    public class DeliveryCostEstimateChallenge : IDeliveryCostEstimateChallengeOperations
    {
        IConfiguration configuration;
        IOfferCodeSeviceOperations offerCodeService;        
        List<Package> packages;
        IDeliveryCostCalculatorOperations deliveryCostCalculator;
        IOfferDiscountCalculatorOperations offerDiscountCalculator;

        public DeliveryCostEstimateChallenge(IConfiguration configuration, List<Package> packages = null) {

            // Pre populate Offer Codes
            this.configuration = configuration;
            offerCodeService = new OfferCodeService(this.configuration);
            this.packages = packages ?? new List<Package>();
        }
        public DeliveryCostEstimateChallenge(IConfiguration configuration, IOfferDiscountCalculatorOperations offerDiscountCalculator=null, IOfferCodeSeviceOperations offerCodeSeviceOperations =null, IDeliveryCostCalculatorOperations deliveryCostCalculator=null, List<Package> packages = null)
        {

            // Pre populate Offer Codes
            this.configuration = configuration;
            offerCodeService = offerCodeSeviceOperations ?? new OfferCodeService(this.configuration);          
            if(deliveryCostCalculator != null)
            {
                this.deliveryCostCalculator = deliveryCostCalculator;
            }

            if(offerDiscountCalculator != null)
            {
                this.offerDiscountCalculator = offerDiscountCalculator;
            }

            this.packages = packages ?? new List<Package>();
        }
        public bool ChallengeSolved()
        {
            return deliveryCostCalculator.GetCalculatedDeliveryCost() > 0 || offerDiscountCalculator.GetCalculatedOfferDiscount() > 0 ? true : false;
        }

        public void StartChallenge()
        {
            var offerCodesConfigFilePath = configuration["Model:OfferCodes:ConfigFilePath"];
            Console.WriteLine("Enter Packages info in the format(first) - base_delivery_cost no_of_packges then on each separate line:- pkg_id1 pkg_weight1_in_kg distance1_in_km offer_code1");
            string input = Console.ReadLine();
            string[] inputs = input.Split(' ');
            double baseDeliveryCost = Convert.ToDouble(inputs[0]);
            int noOfPackages = Convert.ToInt32(inputs[1]);
            packages = new List<Package>();

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

            Console.WriteLine("Output in the format(spaces in between):- packageid discount total_cost");
            foreach(var package in packages)
            {
                deliveryCostCalculator = new DeliveryCostCalculator(baseDeliveryCost, package);
                double deliveryCost = deliveryCostCalculator.GetCalculatedDeliveryCost();
                offerDiscountCalculator = new OfferDiscountCalculator(package, offerCodeService.GetAllOfferCodes(), deliveryCost);
                double discount = offerDiscountCalculator.GetCalculatedOfferDiscount();
                double totalCost = deliveryCostCalculator.GetTotalDeliveryCost(discount);
                Console.WriteLine($"{package.Id} {Math.Truncate(discount)} {totalCost}");
            }

        }

    }
}
