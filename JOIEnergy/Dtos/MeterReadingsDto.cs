using System.Collections.Generic;
using JOIEnergy.Domain;

namespace JOIEnergy.Dtos;

public class MeterReadingsDto
{
    public string SmartMeterId { get; set; }
    public List<ElectricityReading> ElectricityReadings { get; set; }
}