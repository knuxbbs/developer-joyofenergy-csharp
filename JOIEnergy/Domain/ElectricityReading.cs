using System;

namespace JOIEnergy.Domain
{
    public class ElectricityReading
    {
        public ElectricityReading(DateTime time, decimal reading)
        {
            Time = time;
            Reading = reading;
        }
        
        public DateTime Time { get; }
        public decimal Reading { get; }
    }
}
