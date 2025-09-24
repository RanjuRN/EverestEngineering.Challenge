using Everest.CodingChallenge.CourierService.Interfaces;
using Everest.CodingChallenge.CourierService.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.CodingChallenge.CourierService.Services
{
    /// <summary>
    /// Class to schedule the package deliveries based on different vehicles available and criteria like max speed, max carry weight etc.
    /// </summary>
    public class DeliveryPlannerServiceScheduler : IDeliveryPlannerServiceOperations
    {
        
        int vehicleCount;
        List<Package> packages;
        double maxSpeed;
        double maxCarryWeight;
        IConfiguration configuration;

        List<VehiclePackageAssignment> packagesAssignedByVehicle;

        public DeliveryPlannerServiceScheduler (IConfiguration configuration)
        {
           this.configuration = configuration;
           packagesAssignedByVehicle = new List<VehiclePackageAssignment>();
        }
        public DeliveryPlannerServiceScheduler(int vehicleCount, List<Package> packages, double maxSpeed, double maxCarryWeight, IConfiguration configuration)
        {
            this.configuration = configuration;
            var deliveryPlannerServiceOptions = configuration?.GetSection(DeliveryPlannerServiceOptions.DeliveryPlannerServiceConfigHeader).Get<DeliveryPlannerServiceOptions>();

            if (deliveryPlannerServiceOptions == null)
                throw new ArgumentNullException("DeliveryPlannerServiceOptions is not configured properly in appsettings.json");

            //override default config values for vehicle scheduling if provided in constructor differs           
                this.maxCarryWeight = maxCarryWeight != deliveryPlannerServiceOptions.MaxCarryWeight 
                    ? deliveryPlannerServiceOptions.MaxCarryWeight:maxCarryWeight;
                
                this.vehicleCount = vehicleCount != deliveryPlannerServiceOptions.VehicleCount
                    ? deliveryPlannerServiceOptions.VehicleCount
                    : vehicleCount;

                this.maxSpeed = maxSpeed != deliveryPlannerServiceOptions.MaxSpeed
                    ? deliveryPlannerServiceOptions.MaxSpeed
                    : maxSpeed;

                this.maxCarryWeight = maxCarryWeight != deliveryPlannerServiceOptions.MaxCarryWeight
                    ? deliveryPlannerServiceOptions.MaxCarryWeight
                    : maxCarryWeight;

                this.packages = packages;

        }


        // Function to schedule package deliveries and return the approximate time of delivery for each package
        public Dictionary<string, double> SchedulePackageDeliveries(List<Package> packages,int vehicleCount,
                                     double capacity)
        {
            Dictionary<string, double> packageDeliveryByTime = new Dictionary<string, double>();

            var packagesToBeDelivered = new List<Package>(packages);

            var deliveryTimeByPackage = new Dictionary<string, double>();
            var deliveryTimeByVehicle = new PriorityQueue<VehicleByAvailabilityTime, double>(); // A priority queue to get the next available vehicle based on availability time

            for (int i = 1; i <= vehicleCount; i++)
            {
                deliveryTimeByVehicle.Enqueue(new VehicleByAvailabilityTime
                {
                    vehicleId = i,
                    availableAtTime = 0
                }, 0);
            }

            while (packagesToBeDelivered.Count > 0)
            {
                deliveryTimeByVehicle.TryDequeue(out var nextAvailableVehicle, out var vehicleAvailableAtTime);
                var selectedPackageIds = GetSelectedPackageDeliveries(packagesToBeDelivered, capacity);

                double maxDistanceForCurrentDelivery = 0;

                if (selectedPackageIds.Any())
                {
                    packagesToBeDelivered.Where(p => selectedPackageIds.Contains(p.Id)).ToList().ForEach(p =>
                    {
                        maxDistanceForCurrentDelivery = Math.Max(maxDistanceForCurrentDelivery, p.DistanceToDestination);
                    });

                    var selectedPackages = packagesToBeDelivered.Where(p => selectedPackageIds.Contains(p.Id)).ToList();
                    selectedPackages.ForEach(p =>
                    {
                        packageDeliveryByTime[p.Id] = vehicleAvailableAtTime + (p.DistanceToDestination / maxSpeed); // time to reach destination

                    });

                    // Time for round trip based on max distance of the selected packages indiviual distances to destination
                    double vehicleNextAvailableTime = vehicleAvailableAtTime + 2.0 * (maxDistanceForCurrentDelivery / maxSpeed);
                    deliveryTimeByVehicle.Enqueue(new VehicleByAvailabilityTime
                    {
                        vehicleId = nextAvailableVehicle.vehicleId,
                        availableAtTime = vehicleNextAvailableTime
                    }, vehicleNextAvailableTime);

                    packagesToBeDelivered.RemoveAll(p => selectedPackageIds.Contains(p.Id));
                }
                else
                {
                    break; // No more packages can be assigned
                }
            }

            return packageDeliveryByTime;
        }
        public List<string> GetSelectedPackageDeliveries(List<Package> packages, double capacity)
        {
            var packagesByVehicleAssignment = new List<VehiclePackageAssignment>();

            var sortePackagesByWeight = packages.OrderBy(i => i.Weight).ToList();

            HashSet<string> initialPackageIdsBySmallestWeight = new HashSet<string>(); //Build a list of package ids by smallest weight first
            double sumCurrentPackagesWeight = 0;

            foreach (var package in sortePackagesByWeight)
            {
                if (sumCurrentPackagesWeight + package.Weight <= capacity)
                {
                    sumCurrentPackagesWeight += (int)package.Weight;
                    initialPackageIdsBySmallestWeight.Add(package.Id);

                }
                else
                {
                    break;
                }

            }

            var selectedPackages = packages.Where(p => initialPackageIdsBySmallestWeight.Contains(p.Id)).ToList();
            var remainingPackages = packages.Where(p => !initialPackageIdsBySmallestWeight.Contains(p.Id)).ToList();

            double currentTotalDistanceToTravel = 0;
            for (int i = 0; i < initialPackageIdsBySmallestWeight.Count; i++)
            {
                currentTotalDistanceToTravel = selectedPackages.Max(i => i.DistanceToDestination);
                for (int j = 0; j < remainingPackages.Count; j++)
                {
                    double weightDifference = sumCurrentPackagesWeight - sortePackagesByWeight.Where(p => p.Id == initialPackageIdsBySmallestWeight.ElementAt(i)).FirstOrDefault().Weight + remainingPackages[j].Weight;
                   
                    if (weightDifference <= capacity && weightDifference > sumCurrentPackagesWeight)
                    {
                        initialPackageIdsBySmallestWeight.Remove(initialPackageIdsBySmallestWeight.ElementAt(i));
                        initialPackageIdsBySmallestWeight.Add(remainingPackages[j].Id);
                        sumCurrentPackagesWeight = weightDifference;

                        currentTotalDistanceToTravel = Math.Max(currentTotalDistanceToTravel, sortePackagesByWeight.Where(p => p.Id == remainingPackages[j].Id).FirstOrDefault().Weight);
                    }
                    else
                        continue;
                                    
                }
            }

            return initialPackageIdsBySmallestWeight.ToList();
        }

        /// <summary>
        /// Get the indiviual package delivery time based on the delivery scheduling
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, double> GetPackagesDeliveriesByDeliveryTime()
        {           
            var selectedPackages = SchedulePackageDeliveries(packages,vehicleCount, maxCarryWeight);
            return selectedPackages;
        }

    }
}
