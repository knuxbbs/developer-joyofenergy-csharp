using System;
using System.Collections.Generic;
using JOIEnergy.Domain;

namespace JOIEnergy.Generator
{
    public static class ElectricityReadingGenerator
    {
        public static List<ElectricityReading> Generate(int number)
        {
            var readings = new List<ElectricityReading>();
            var random = new Random();
            
            for (int i = 0; i < number; i++)
            {
                readings.Add(new ElectricityReading(DateTime.Now.AddSeconds(-i * 10), (decimal) random.NextDouble()));
            }
            
            readings.Sort((reading1, reading2) => reading1.Time.CompareTo(reading2.Time));
            return readings;
        }
    }
}
