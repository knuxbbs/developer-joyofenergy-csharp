using System.Collections.Generic;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly Dictionary<string, List<ElectricityReading>> _meterAssociatedReadings;
        
        public MeterReadingService(Dictionary<string, List<ElectricityReading>> meterAssociatedReadings)
        {
            _meterAssociatedReadings = meterAssociatedReadings;
        }

        public List<ElectricityReading> GetElectricityReadings(string smartMeterId)
        {
            return _meterAssociatedReadings.ContainsKey(smartMeterId)
                ? _meterAssociatedReadings[smartMeterId]
                : new List<ElectricityReading>();
        }

        public void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings) {
            if (!_meterAssociatedReadings.ContainsKey(smartMeterId)) {
                _meterAssociatedReadings.Add(smartMeterId, new List<ElectricityReading>());
            }

            electricityReadings.ForEach(electricityReading => _meterAssociatedReadings[smartMeterId].Add(electricityReading));
        }
    }
}
