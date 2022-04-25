using System.Collections.Generic;
using JOIEnergy.Domain;

namespace JOIEnergy.Services
{
    public interface IMeterReadingService
    {
        List<ElectricityReading> GetReadings(string smartMeterId);
        List<ElectricityReading> GetLastWeekReadings(string smartMeterId);
        void StoreReadings(string smartMeterId, List<ElectricityReading> electricityReadings);
    }
}