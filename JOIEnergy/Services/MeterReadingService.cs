using System.Collections.Generic;
using System.Linq;
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

        public List<ElectricityReading> GetReadings(string smartMeterId)
        {
            return _meterAssociatedReadings.ContainsKey(smartMeterId)
                ? _meterAssociatedReadings[smartMeterId]
                : new List<ElectricityReading>();
        }
        
        public List<ElectricityReading> GetLastWeekReadings(string smartMeterId)
        {
            //TODO: implementar testes
            if (!_meterAssociatedReadings.ContainsKey(smartMeterId))
            {
                return new List<ElectricityReading>();
            }

            ElectricityReadingInterval previousWeekInterval = ElectricityReadingInterval.GetPreviousWeekInterval();
            List<ElectricityReading> meterReadings = _meterAssociatedReadings[smartMeterId];

            return meterReadings
                .Where(x => x.Time >= previousWeekInterval.Start && x.Time < previousWeekInterval.End)
                .ToList();
        }

        public void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings) {
            if (!_meterAssociatedReadings.ContainsKey(smartMeterId)) {
                _meterAssociatedReadings.Add(smartMeterId, new List<ElectricityReading>());
            }

            electricityReadings.ForEach(electricityReading => _meterAssociatedReadings[smartMeterId].Add(electricityReading));
        }
    }
}
