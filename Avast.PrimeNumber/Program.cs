using System.Threading.Tasks;
using Avast.PrimeNumber.Builders;
using Avast.PrimeNumber.Checkers;
using Avast.PrimeNumber.Generators;
using Avast.PrimeNumber.Managers;
using Avast.PrimeNumber.Services;
using Avast.PrimeNumber.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Avast.PrimeNumber
{
    class Program
    {
		static Task Main(string[] args)
		{
			using IHost host = CreateHostBuilder(args).Build();

			return host.RunAsync();
		}

		static IHostBuilder CreateHostBuilder(string[] args) =>
				Host.CreateDefaultBuilder(args)
					.ConfigureHostConfiguration(configHost => configHost.AddEnvironmentVariables(prefix: "NETCORE_"))
					.ConfigureAppConfiguration((hostingContext, config) =>
					{
						config
						.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
						.AddJsonFile(
							$"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
							optional: true,
							reloadOnChange: true);

						if (args != null)
						{
							config.AddCommandLine(args); // could add keys to args and use config to store them
						}
					})
					.ConfigureLogging((hostingContext, logging) =>
					{
						logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
						logging.AddConsole();
					})
					.ConfigureServices((hostingContext, services) =>
					{
						services.AddOptions();

						var configuration = hostingContext.Configuration;
						var appSettingsSection = configuration.GetSection("AppSettings");
						services.Configure<AppSettings>(appSettingsSection);

						services.AddTransient<IPrintManager, PrintManager>();
						services.AddTransient<IGridBuilder, GridBuilder>();
						services.AddTransient<IPrimeNumberChecker, PrimeNumberChecker>();
						services.AddTransient<IPrimeNumberGenerator, PrimeNumberGenerator>();
						services.AddTransient<IPrimeNumberService, PrimeNumberService>();

						services.AddHostedService<HostedPrimeNumberService>();
					});
	}
}
