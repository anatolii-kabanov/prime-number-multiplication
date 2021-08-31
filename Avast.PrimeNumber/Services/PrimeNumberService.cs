using System;
using System.Threading;
using System.Threading.Tasks;
using Avast.PrimeNumber.Builders;
using Avast.PrimeNumber.Generators;
using Avast.PrimeNumber.Managers;
using Microsoft.Extensions.Logging;

namespace Avast.PrimeNumber.Services
{
    public class PrimeNumberService : IPrimeNumberService
    {
        private readonly ILogger<PrimeNumberService> _logger;
        private readonly IPrimeNumberGenerator _primeNumberGenerator;
        private readonly IGridBuilder _gridBuilder;
        private readonly IPrintManager _printManager;

        public PrimeNumberService(
            IPrimeNumberGenerator primeNumberGenerator, IGridBuilder gridBuilder, IPrintManager printManager, ILogger<PrimeNumberService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _primeNumberGenerator = primeNumberGenerator ?? throw new ArgumentNullException(nameof(primeNumberGenerator));
            _gridBuilder = gridBuilder ?? throw new ArgumentNullException(nameof(gridBuilder));
            _printManager = printManager ?? throw new ArgumentNullException(nameof(printManager));
        }

        public async Task<int> RunAsync(string quantity, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(quantity))
            {
                _logger.LogError($"{nameof(quantity)} should contain number");
                throw new ArgumentNullException(nameof(quantity));
            }
            if (!int.TryParse(quantity, out var quantityNumber))
            {
                _logger.LogError($"{nameof(quantity)} should be integer number");
                throw new ArgumentException(nameof(quantity));
            }
            if (quantityNumber < 1)
            {
                _logger.LogError($"{nameof(quantity)} should be greater than zero");
                throw new ArgumentOutOfRangeException(nameof(quantity));
            }

            var primeNumbers = await _primeNumberGenerator.Next(quantityNumber).ConfigureAwait(false);

            _logger.LogDebug($"{string.Join(", ", primeNumbers)}");
            _logger.LogInformation($"Found first {primeNumbers.Length} prime numbers.");

            var multiplicationGrid = _gridBuilder.MultiplicationGrid(primeNumbers);

            _logger.LogDebug($"");
            _logger.LogInformation($"Multiplication grid created.");

            _printManager.ToConsole(multiplicationGrid);

            return 0;
        }
    }
}
