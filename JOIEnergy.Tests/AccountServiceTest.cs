using System;
using System.Collections.Generic;
using JOIEnergy.Enums;
using JOIEnergy.Services;
using Xunit;

namespace JOIEnergy.Tests
{
    public class AccountServiceTest
    {
        private const Supplier PricePlanId = Supplier.PowerForEveryone;
        private const string SmartMeterId = "smart-meter-id";

        private readonly AccountService _accountService;

        public AccountServiceTest()
        {
            var smartMeterToPricePlanAccounts = new Dictionary<string, Supplier> {{SmartMeterId, PricePlanId}};

            _accountService = new AccountService(smartMeterToPricePlanAccounts);
        }

        [Fact]
        public void GivenTheSmartMeterIdReturnsThePricePlanId()
        {
            var result = _accountService.GetPricePlanIdForSmartMeterId(SmartMeterId);
            Assert.Equal(PricePlanId, result);
        }

        [Fact]
        public void GivenAnUnknownSmartMeterIdReturnsANullSupplier()
        {
            var result = _accountService.GetPricePlanIdForSmartMeterId("bob-carolgees");
            Assert.Equal(Supplier.NullSupplier, result);
        }
    }
}
