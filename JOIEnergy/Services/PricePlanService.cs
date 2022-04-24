using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public class PricePlanService : IPricePlanService
    {
        private readonly List<PricePlan> _pricePlans;
        private IMeterReadingService _meterReadingService;

        public PricePlanService(List<PricePlan> pricePlan, IMeterReadingService meterReadingService)
        {
            _pricePlans = pricePlan;
            _meterReadingService = meterReadingService;
        }

        public Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId)
        {
            List<ElectricityReading> electricityReadings = _meterReadingService.GetElectricityReadings(smartMeterId);
            var meterReadings = new MeterReadings(smartMeterId, electricityReadings);

            return electricityReadings.Any()
                ? _pricePlans.ToDictionary(plan => plan.EnergySupplier.ToString(),
                    plan => meterReadings.GetAverageCost(plan))
                : new Dictionary<string, decimal>();
        }
    }
}
