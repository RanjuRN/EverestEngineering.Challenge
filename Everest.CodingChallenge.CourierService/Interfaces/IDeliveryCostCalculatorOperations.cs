using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.CodingChallenge.CourierService.Interfaces
{
    public interface IDeliveryCostCalculatorOperations
    {
        public double GetCalculatedDeliveryCost();
        public double GetTotalDeliveryCost(double discount);
    }
}
