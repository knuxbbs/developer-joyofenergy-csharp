using System;
using JOIEnergy.Domain;
using Xunit;

namespace JOIEnergy.Tests;

public class ElectricityReadingIntervalTest
{
    [Fact]
    public void GetLastWeekFromToday()
    {
        Assert.Equal(new DateTime(2021, 9, 26),
            ElectricityReadingInterval.GetPreviousWeekInterval(new DateTime(2021, 10, 5)).Start);
        Assert.Equal(new DateTime(2021, 10, 3),
            ElectricityReadingInterval.GetPreviousWeekInterval(new DateTime(2021, 10, 5)).End);
    }
}