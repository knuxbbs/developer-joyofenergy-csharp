using System.Collections.Generic;
using System.Linq;

namespace JOIEnergy.Domain
{
    public class MeterReadings
    {
        public MeterReadings(string smartMeterId, List<ElectricityReading> electricityReadings)
        {
            SmartMeterId = smartMeterId;
            ElectricityReadings = electricityReadings;
        }
        
        public string SmartMeterId { get; set; }
        public List<ElectricityReading> ElectricityReadings { get; set; }

        public decimal GetAverageCost(PricePlan pricePlan)
        {
            return CalculateAverageReading() / CalculateTimeElapsed() * pricePlan.UnitRate;
        }
        
        private decimal CalculateAverageReading()
        {
            var newSummedReadings = ElectricityReadings.Sum(x => x.Reading);

            return newSummedReadings / ElectricityReadings.Count;
        }

        private decimal CalculateTimeElapsed()
        {
            var first = ElectricityReadings.Min(reading => reading.Time);
            var last = ElectricityReadings.Max(reading => reading.Time);

            return (decimal)(last - first).TotalHours;
        }
    }
}
