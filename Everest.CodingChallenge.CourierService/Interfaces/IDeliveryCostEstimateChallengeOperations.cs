using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.CodingChallenge.CourierService.Interfaces
{
    public interface IDeliveryCostEstimateChallengeOperations : IChallengeOperations
    {
        public bool ChallengeSolved();

    }
}
