using System.Collections.Generic;
using System.Linq;
using JOIEnergy.Domain;
using JOIEnergy.Enums;
using JOIEnergy.Generator;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace JOIEnergy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var readings = GenerateMeterElectricityReadings();

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IMeterReadingService, MeterReadingService>();
            services.AddTransient<IPricePlanService, PricePlanService>();
            services.AddSingleton(_ => readings);
            services.AddSingleton(_ => SamplePricePlans);
            services.AddSingleton(_ => SmartMeterToPricePlanAccounts);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        private static Dictionary<string, List<ElectricityReading>> GenerateMeterElectricityReadings() {
            var readings = new Dictionary<string, List<ElectricityReading>>();
            var smartMeterIds = SmartMeterToPricePlanAccounts.Select(x => x.Key);

            foreach (var smartMeterId in smartMeterIds)
            {
                readings.Add(smartMeterId, ElectricityReadingGenerator.Generate(20));
            }
            
            return readings;
        }

        private static Dictionary<string, Supplier> SmartMeterToPricePlanAccounts =>
            new()
            {
                {"smart-meter-0", Supplier.DrEvilsDarkEnergy},
                {"smart-meter-1", Supplier.TheGreenEco},
                {"smart-meter-2", Supplier.DrEvilsDarkEnergy},
                {"smart-meter-3", Supplier.PowerForEveryone},
                {"smart-meter-4", Supplier.TheGreenEco}
            };

        private static readonly List<PricePlan> SamplePricePlans = new()
        {
            new PricePlan
            {
                EnergySupplier = Supplier.DrEvilsDarkEnergy,
                UnitRate = 10m,
            },
            new PricePlan
            {
                EnergySupplier = Supplier.TheGreenEco,
                UnitRate = 2m,
            },
            new PricePlan
            {
                EnergySupplier = Supplier.PowerForEveryone,
                UnitRate = 1m,
            }
        };
    }
}
