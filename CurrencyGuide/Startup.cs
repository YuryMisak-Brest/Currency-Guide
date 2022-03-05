using System;
using System.Runtime.InteropServices;
using AutoMapper;
using CurrencyGuide.ExternalServices;
using CurrencyGuide.Filters;
using CurrencyGuide.Models.Interfaces;
using CurrencyGuide.Storage;
using CurrencyGuide.Service;
using CurrencyGuide.Storage.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Currency = CurrencyGuide.Storage.Domain.Currency;

namespace CurrencyGuide
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
			services.AddControllers(options =>
			{
				options.Filters.Add<ExceptionFilter>();
			});
			// In production, the Angular files will be served from this directory
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp/dist";
			});
			services.AddTransient<ICurrenciesService, CurrenciesService>();
			services.AddTransient<ILoadDataService, LoadDataService>(); 
			services.AddTransient<IConversionRatesService, ConversionRateService>();
			services.AddTransient<IExternalCurrencyService, FastForexService>(_=> new FastForexService(Configuration.GetValue<string>("apiUrl"), Configuration.GetValue<string>("apiKey"))); 
			services.AddTransient<IRepository<Currency>, Repository<Currency>>();
			services.AddTransient<IRepository<ExchangeRate>, Repository<ExchangeRate>>();
			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new CgProfile());
			});

			IMapper mapper = mappingConfig.CreateMapper();
			services.AddSingleton(mapper);

			services.AddMvc();
			services.AddDbContext<CgContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CgDatabase")));

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			if (!env.IsDevelopment())
			{
				app.UseSpaStaticFiles();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller}/{action=Index}/{id?}");
			});

			app.UseSpa(spa =>
			{
				// To learn more about options for serving an Angular SPA from ASP.NET Core,
				// see https://go.microsoft.com/fwlink/?linkid=864501

				spa.Options.SourcePath = "ClientApp";

				if (env.IsDevelopment())
				{
					spa.UseAngularCliServer(npmScript: "start");
				}
			});

			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetRequiredService<CgContext>();
				if (context.Database.EnsureCreated())
				{
					var loadDataService = serviceScope.ServiceProvider.GetRequiredService<ILoadDataService>();
					loadDataService.Setup(DateTime.Today.AddDays(-Configuration.GetValue<int>("initialSetupBackwardsDays")));
				}
			}
		}
	}
}
