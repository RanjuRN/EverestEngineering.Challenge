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
        IIOServiceOperations consoleOperations;

        public DeliveryCostEstimateChallenge(IConfiguration configuration, List<Package> packages = null, IIOServiceOperations consoleOperations = null)
        {

            // Pre populate Offer Codes
            this.configuration = configuration;
            offerCodeService = new OfferCodeService(this.configuration);
            this.packages = packages ?? new List<Package>();
            this.consoleOperations = consoleOperations ??  new IOConsoleOperations();

        }
        public DeliveryCostEstimateChallenge(IConfiguration configuration, IOfferDiscountCalculatorOperations offerDiscountCalculator=null, IOfferCodeSeviceOperations offerCodeSeviceOperations =null, IDeliveryCostCalculatorOperations deliveryCostCalculator=null, List<Package> packages = null,IIOServiceOperations consoleOperations = null)
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

           this.consoleOperations = consoleOperations ?? new IOConsoleOperations() ;            
           this.packages = packages ?? new List<Package>();
            
        }
        public bool ChallengeSolved()
        {
            return deliveryCostCalculator.GetCalculatedDeliveryCost() > 0 || offerDiscountCalculator.GetCalculatedOfferDiscount() > 0 ? true : false;
        }

        public void StartChallenge()
        {
            var offerCodesConfigFilePath = configuration["Model:OfferCodes:ConfigFilePath"];
            consoleOperations.WriteLine("Enter Packages info in the format(first) - base_delivery_cost no_of_packges then on each separate line:- pkg_id1 pkg_weight1_in_kg distance1_in_km offer_code1");

            double baseDeliveryCost;
            int noOfPackages;
            GetInputPackageParameters(out baseDeliveryCost, out noOfPackages);

            packages = ReadPackages(noOfPackages, consoleOperations);

            consoleOperations.WriteLine("Output in the format(spaces in between):- packageid discount total_cost");
            var results = CalculateDeliveryCosts(packages, baseDeliveryCost);

            foreach (var result in results)
            {
                consoleOperations.WriteLine($"{result.PackageId} {Math.Truncate(result.Discount)} {result.TotalCost}");
            }
        }

        /// <summary>
        /// Reads package input from the provided IIOServiceOperations.
        /// </summary>
        public List<Package> ReadPackages(int noOfPackages, IIOServiceOperations ioService)
        {

            var packageList = new List<Package>();
            for (int i = 0; i < noOfPackages; i++)
            {
                string packageInput = ioService.ReadLine();
                string[] packageInputs = packageInput.Split(' ');
                var package = new Package()
                {
                    Id = packageInputs[0],
                    Weight = Convert.ToDouble(packageInputs[1]),
                    DistanceToDestination = Convert.ToDouble(packageInputs[2]),
                    ApplicableOfferCode = packageInputs.Length > 3 ? packageInputs[3] : null
                };
                packageList.Add(package);
            }
            return packageList;
        }

        /// <summary>
        /// Calculates delivery costs and discounts for a list of packages.
        /// </summary>
        private List<DeliveryResult> CalculateDeliveryCosts(List<Package> packages, double baseDeliveryCost)
        {
            var results = new List<DeliveryResult>();
            var offerCodes = offerCodeService.GetAllOfferCodes();

            foreach (var package in packages)
            {
                var costCalculator = new DeliveryCostCalculator(baseDeliveryCost, package);
                double deliveryCost = costCalculator.GetCalculatedDeliveryCost();

                var discountCalculator = new OfferDiscountCalculator(package, offerCodes, deliveryCost);
                double discount = discountCalculator.GetCalculatedOfferDiscount();

                double totalCost = costCalculator.GetTotalDeliveryCost(discount);

                results.Add(new DeliveryResult
                {
                    PackageId = package.Id,
                    Discount = discount,
                    TotalCost = totalCost
                });
            }
            return results;
        }

        /// <summary>
        /// DTO for delivery result.
        /// </summary>
        private class DeliveryResult
        {
            public string PackageId { get; set; }
            public double Discount { get; set; }
            public double TotalCost { get; set; }
        }

        public  void GetInputPackageParameters(out double baseDeliveryCost, out int noOfPackages)
        {
            //string input = Console.ReadLine();
            string input = consoleOperations.ReadLine();
            string[] inputs = input.Split(' ');
            baseDeliveryCost = Convert.ToDouble(inputs[0]);
            noOfPackages = Convert.ToInt32(inputs[1]);
        }


    }
}
