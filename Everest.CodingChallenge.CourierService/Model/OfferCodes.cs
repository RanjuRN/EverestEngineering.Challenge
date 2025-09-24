using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.CodingChallenge.CourierService.Model
{
    public class OfferCodes
    {
        public string Code { get; set; }
        public double DiscountPercentage { get; set; }
        public double MinWeight { get; set; }
        public double MaxWeight { get; set; }
        public double MinDistance { get; set; }
        public double MaxDistance { get; set; }
    }
}
