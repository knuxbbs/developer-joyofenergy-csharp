using JOIEnergy.Controllers;
using JOIEnergy.Domain;
using JOIEnergy.Enums;
using JOIEnergy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Newtonsoft.Json.Linq;

namespace JOIEnergy.Tests
{
    public class PricePlanComparisonTest
    {
        private const string SmartMeterId = "smart-meter-id";
        
        private readonly PricePlanComparatorController _controller;
        private readonly MeterReadingService _meterReadingService;

        public PricePlanComparisonTest()
        {
            var readings = new Dictionary<string, List<ElectricityReading>>();
            _meterReadingService = new MeterReadingService(readings);

            var pricePlans = new List<PricePlan>
            {
                new() {EnergySupplier = Supplier.DrEvilsDarkEnergy, UnitRate = 10},
                new() {EnergySupplier = Supplier.TheGreenEco, UnitRate = 2},
                new() {EnergySupplier = Supplier.PowerForEveryone, UnitRate = 1}
            };
            
            var pricePlanService = new PricePlanService(pricePlans, _meterReadingService);
            var accountService = new AccountService(new Dictionary<string, Supplier>());
            _controller = new PricePlanComparatorController(pricePlanService, accountService);
        }

        [Fact]
        public void ShouldCalculateCostForMeterReadingsForEveryPricePlan()
        {
            var electricityReadings = new List<ElectricityReading>
            {
                new(DateTime.Now.AddHours(-1), 15.0m),
                new(DateTime.Now, 5.0m)
            };

            _meterReadingService.StoreReadings(SmartMeterId, electricityReadings);

            Dictionary<string, decimal> result =
                _controller.CalculatedCostForEachPricePlan(SmartMeterId).Value as Dictionary<string, decimal>;

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(100m, result[Supplier.DrEvilsDarkEnergy.ToString()], 3);
            Assert.Equal(20m, result[Supplier.TheGreenEco.ToString()], 3);
            Assert.Equal(10m, result[Supplier.PowerForEveryone.ToString()], 3);
        }

        [Fact]
        public void ShouldRecommendCheapestPricePlansNoLimitForMeterUsage()
        {
            _meterReadingService.StoreReadings(SmartMeterId, new List<ElectricityReading> {
                new(DateTime.Now.AddMinutes(-30), 35m),
                new(DateTime.Now, 3m)
            });

            object result = _controller.RecommendCheapestPricePlans(SmartMeterId).Value;
            var recommendations = ((IEnumerable<KeyValuePair<string, decimal>>)result).ToList();

            Assert.Equal("" + Supplier.PowerForEveryone, recommendations[0].Key);
            Assert.Equal("" + Supplier.TheGreenEco, recommendations[1].Key);
            Assert.Equal("" + Supplier.DrEvilsDarkEnergy, recommendations[2].Key);
            Assert.Equal(38m, recommendations[0].Value, 3);
            Assert.Equal(76m, recommendations[1].Value, 3);
            Assert.Equal(380m, recommendations[2].Value, 3);
            Assert.Equal(3, recommendations.Count);
        }

        [Fact]
        public void ShouldRecommendLimitedCheapestPricePlansForMeterUsage()
        {
            _meterReadingService.StoreReadings(SmartMeterId, new List<ElectricityReading>
            {
                new(DateTime.Now.AddMinutes(-45), 5m),
                new(DateTime.Now, 20m)
            });

            object result = _controller.RecommendCheapestPricePlans(SmartMeterId, 2).Value;
            var recommendations = ((IEnumerable<KeyValuePair<string, decimal>>)result).ToList();

            Assert.Equal("" + Supplier.PowerForEveryone, recommendations[0].Key);
            Assert.Equal("" + Supplier.TheGreenEco, recommendations[1].Key);
            Assert.Equal(16.667m, recommendations[0].Value, 3);
            Assert.Equal(33.333m, recommendations[1].Value, 3);
            Assert.Equal(2, recommendations.Count);
        }

        [Fact]
        public void ShouldRecommendCheapestPricePlansMoreThanLimitAvailableForMeterUsage()
        {
            _meterReadingService.StoreReadings(SmartMeterId, new List<ElectricityReading>
            {
                new(DateTime.Now.AddMinutes(-30), 35m),
                new(DateTime.Now, 3m)
            });

            object result = _controller.RecommendCheapestPricePlans(SmartMeterId, 5).Value;
            var recommendations = ((IEnumerable<KeyValuePair<string, decimal>>)result).ToList();

            Assert.Equal(3, recommendations.Count);
        }

        [Fact]
        public void GivenNoMatchingMeterIdShouldReturnNotFound()
        {
            Assert.Equal(404, _controller.CalculatedCostForEachPricePlan("not-found").StatusCode);
        }
    }
}
