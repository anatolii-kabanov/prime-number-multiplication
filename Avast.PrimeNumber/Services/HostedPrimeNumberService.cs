using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Avast.PrimeNumber.Services
{
    public sealed class HostedPrimeNumberService : IHostedPrimeNumberService
	{
		private readonly IHostApplicationLifetime _appLifetime;
		private readonly IPrimeNumberService _primeNumberService;
		private readonly ILogger<HostedPrimeNumberService> _logger;
        private int? _resultCode;

        public HostedPrimeNumberService(IHostApplicationLifetime appLifetime, IPrimeNumberService primeNumberService, ILogger<HostedPrimeNumberService> logger)
		{
			_appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
			_primeNumberService = primeNumberService ?? throw new ArgumentNullException(nameof(primeNumberService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
			_logger.LogInformation("Starting prime number service...");
			var args = Environment.GetCommandLineArgs().Skip(1).ToArray();
			_logger.LogDebug($"Starting with arguments: {string.Join(" ", args)}");

			if (args.Length != 1)
			{
				_logger.LogInformation("To start program you should provide 1 argument: the quantity of prime numbers that you want to multiply");
				_appLifetime.StopApplication();
			}
			else
			{
				_appLifetime.ApplicationStarted.Register(async () =>
				{
					try
					{
						_resultCode = await _primeNumberService.RunAsync(args[0], cancellationToken).ConfigureAwait(false);
					}
					catch (Exception ex)
					{
						_logger.LogError("Error while executing prime number program, finishing work...", ex);
						_resultCode = 1;
					}
					finally
					{
						_appLifetime.StopApplication();
					}
				});
			}

			return Task.CompletedTask;
		}

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping prime number service...");

            Environment.ExitCode = _resultCode.GetValueOrDefault(-1);

            _logger.LogDebug($"Exit code: {Environment.ExitCode}");

            return Task.CompletedTask;
        }
    }
}
