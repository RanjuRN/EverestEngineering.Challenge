using Everest.CodingChallenge.CourierService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.CodingChallenge.CourierService.Interfaces
{
    public interface IDeliveryPlannerServiceOperations
    {
        public Dictionary<string, double> GetPackagesDeliveriesByDeliveryTime();
        public List<string> GetSelectedPackageDeliveries(List<Package> packages, double capacity);
        public Dictionary<string, double> SchedulePackageDeliveries(List<Package> packages, int vehicleCount, double capacity);

    }
}
