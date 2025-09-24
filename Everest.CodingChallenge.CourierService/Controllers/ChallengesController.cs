using Everest.CodingChallenge.CourierService.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.CodingChallenge.CourierService.Controller
{
    /// <summary>
    /// Class to control the flow of challenges and call the individual challenge
    /// </summary>
    public class ChallengesController: IChallengesControllerOperations
    {
        IConfiguration configuration;
        IChallengeOperations challenge1,challenge2;

        public ChallengesController(IConfiguration config, IDeliveryCostEstimateChallengeOperations challenge1, IDeliveryTimeEstimateChallengeOperations challenge2)
        {
            configuration = config;
            this.challenge1 = challenge1;
            this.challenge2 = challenge2;
        }


        public void OrchestrateChallenges()
        {
            // First Challenge inputs and response       
            //challenge1.StartChallenge();
            // Second Challenge inputs and response
            challenge2.StartChallenge();
        }

    }
}
