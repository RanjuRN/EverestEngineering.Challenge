using Everest.CodingChallenge.CourierService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.CodingChallenge.CourierService.Interfaces
{
    public interface IOfferCodeSeviceOperations
    {
        public void AddOfferCode(OfferCodes offercode);
        public List<OfferCodes> GetAllOfferCodes();
    }
}
