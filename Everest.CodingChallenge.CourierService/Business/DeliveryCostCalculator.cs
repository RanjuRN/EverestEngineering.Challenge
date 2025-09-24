using Everest.CodingChallenge.CourierService.Interfaces;
using Everest.CodingChallenge.CourierService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.CodingChallenge.CourierService.Business
{
    public class DeliveryCostCalculator : IDeliveryCostCalculatorOperations
    {
        double baseDeliveryCost { get; set; }
        Package package { get; set; }

       public DeliveryCostCalculator(double baseDeliveryCost, Model.Package package)
        {
            this.baseDeliveryCost = baseDeliveryCost;
            this.package = package;
        }

        public double GetCalculatedDeliveryCost()
        {
            return baseDeliveryCost + (package.Weight * 10) + (package.DistanceToDestination * 5);
        }

        public double GetTotalDeliveryCost(double discount)
        {
            return GetCalculatedDeliveryCost() - discount;
        }
    }
}
