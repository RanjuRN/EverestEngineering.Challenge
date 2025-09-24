using Everest.CodingChallenge.CourierService.Interfaces;
using Everest.CodingChallenge.CourierService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Everest.CodingChallenge.CourierService.Business
{
    public class OfferDiscountCalculator : IOfferDiscountCalculatorOperations
    {
        Package package { get; set; }
        List<OfferCodes> offerCodes { get; set; }

        double discount;
        double deliveryCost;

        public OfferDiscountCalculator(Model.Package package, List<Model.OfferCodes> offerCodes, double deliveryCost)
        {
            this.package = package;
            this.offerCodes = offerCodes;
            discount = 0;
            this.deliveryCost = deliveryCost;

        }

        /// <summary>
        /// Get calculated offer discount based on the package details and applicable offer code
        /// </summary>
        /// <returns></returns>
        public double GetCalculatedOfferDiscount()
        {
            double discount = 0;
            if (package.ApplicableOfferCode != null)
            {
                var offer = offerCodes.Where(x => x.Code == package.ApplicableOfferCode).FirstOrDefault();

                if (offer != null)
                {
                    if (package.Weight >= offer.MinWeight && package.Weight <= offer.MaxWeight
                        && package.DistanceToDestination >= offer.MinDistance
                        && package.DistanceToDestination <= offer.MaxDistance)
                    {
                        discount = (offer.DiscountPercentage / 100) * deliveryCost;
                    }
                }

            }
            return discount;
        }



    }
}
