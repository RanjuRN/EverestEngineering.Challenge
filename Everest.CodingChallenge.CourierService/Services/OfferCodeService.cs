using Everest.CodingChallenge.CourierService.Interfaces;
using Everest.CodingChallenge.CourierService.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Everest.CodingChallenge.CourierService.Services
{
    /// <summary>
    /// Class to manage the offer codes and related operations
    /// </summary>
    public class OfferCodeService : IOfferCodeSeviceOperations
    {
        private List<OfferCodes> offerCodes;
        IConfiguration configuration;

        public OfferCodeService(IConfiguration configuration)
        {
            // Pre populate Offer Codes
            offerCodes = new List<Model.OfferCodes>()
            {
                new Model.OfferCodes()
                {
                    Code="OFR001",
                    DiscountPercentage=10,
                    MinWeight=70,
                    MaxWeight=200,
                    MinDistance=0,
                    MaxDistance=199
                },
                new Model.OfferCodes()
                {
                    Code="OFR002",
                    DiscountPercentage=7,
                    MinWeight=100,
                    MaxWeight=250,
                    MinDistance=50,
                    MaxDistance=150
                },
                new Model.OfferCodes()
                {
                    Code="OFFR002",
                    DiscountPercentage=7,
                    MinWeight=100,
                    MaxWeight=250,
                    MinDistance=50,
                    MaxDistance=150
                },
                new Model.OfferCodes()
                {
                    Code="OFR003",
                    DiscountPercentage=5,
                    MinWeight=10,
                    MaxWeight=150,
                    MinDistance=50,
                    MaxDistance=250
                }
            };

            this.configuration = configuration;
            var offerCodesDataConfigFilePath = configuration.GetSection("Model:OfferCodes").GetValue<string>("ConfigFilePath");

            if(!string.IsNullOrEmpty(offerCodesDataConfigFilePath))
            {
                // Load offer codes from configuration file if path is provided
                offerCodesDataConfigFilePath = Path.Combine(AppContext.BaseDirectory, offerCodesDataConfigFilePath);
                string offerCodesData = File.ReadAllText(offerCodesDataConfigFilePath);
                var offerCodes = JsonSerializer.Deserialize<List<Model.OfferCodes>>(offerCodesData);

            }

        }

        public List<Model.OfferCodes> GetAllOfferCodes()
        {
            return offerCodes;
        }

        public void AddOfferCode(OfferCodes offercode)
        {
            if(offercode != null && !string.IsNullOrEmpty(offercode.Code))
            {
                offerCodes.Add(offercode);
            }
           
        }
        
        
    }



}
