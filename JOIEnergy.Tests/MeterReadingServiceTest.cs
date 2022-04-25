using System;
using System.Collections.Generic;
using JOIEnergy.Services;
using JOIEnergy.Domain;
using Xunit;

namespace JOIEnergy.Tests
{
    public class MeterReadingServiceTest
    {
        private const string SmartMeterId = "smart-meter-id";

        private readonly MeterReadingService _meterReadingService;

        public MeterReadingServiceTest()
        {
            _meterReadingService = new MeterReadingService(new Dictionary<string, List<ElectricityReading>>());

            _meterReadingService.StoreReadings(SmartMeterId, new List<ElectricityReading>
            {
                new(DateTime.Now.AddMinutes(-30), 35m),
                new(DateTime.Now.AddMinutes(-15), 30m)
            });
        }

        [Fact]
        public void GivenMeterIdThatDoesNotExistShouldReturnNull() {
            Assert.Empty(_meterReadingService.GetReadings("unknown-id"));
        }

        [Fact]
        public void GivenMeterReadingThatExistsShouldReturnMeterReadings()
        {
            _meterReadingService.StoreReadings(SmartMeterId, new List<ElectricityReading>
            {
                new(DateTime.Now, 25m)
            });

            var electricityReadings = _meterReadingService.GetReadings(SmartMeterId);

            Assert.Equal(3, electricityReadings.Count);
        }

    }
}
