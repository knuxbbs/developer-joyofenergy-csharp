using System;
using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;
using JOIEnergy.Dtos;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JOIEnergy.Controllers
{
    [Route("readings")]
    public class MeterReadingController : Controller
    {
        private readonly IMeterReadingService _meterReadingService;

        public MeterReadingController(IMeterReadingService meterReadingService)
        {
            _meterReadingService = meterReadingService;
        }
        
        // POST api/values
        [HttpPost ("store")]
        public IActionResult Post([FromBody] MeterReadingsDto dto)
        {
            if (!IsMeterReadingsValid(dto))
            {
                return BadRequest("Internal Server Error");
            }

            _meterReadingService.StoreReadings(dto.SmartMeterId, dto.ElectricityReadings);

            return Ok();
        }

        private bool IsMeterReadingsValid(MeterReadingsDto dto)
        {
            String smartMeterId = dto.SmartMeterId;
            List<ElectricityReading> electricityReadings = dto.ElectricityReadings;
            
            return smartMeterId != null && smartMeterId.Any()
                    && electricityReadings != null && electricityReadings.Any();
        }

        [HttpGet("read/{smartMeterId}")]
        public ActionResult<List<ElectricityReading>> GetReading(string smartMeterId) {
            return Ok(_meterReadingService.GetElectricityReadings(smartMeterId));
        }
    }
}
