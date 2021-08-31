using System;
using System.Threading;
using System.Threading.Tasks;
using Avast.PrimeNumber.Checkers;
using Microsoft.Extensions.Logging;

namespace Avast.PrimeNumber.Generators
{
    public class PrimeNumberGenerator : IPrimeNumberGenerator
    {
        private readonly IPrimeNumberChecker _primeNumberChecker;
        private readonly ILogger<PrimeNumberGenerator> _logger;

        public PrimeNumberGenerator(IPrimeNumberChecker primeNumberChecker, ILogger<PrimeNumberGenerator> logger)
        {
            _primeNumberChecker = primeNumberChecker ?? throw new ArgumentNullException(nameof(primeNumberChecker));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int[]> Next(int quantity, CancellationToken cancellationToken = default)
        {
            if (quantity < 1) throw new ArgumentOutOfRangeException(nameof(quantity));

            _logger.LogDebug($"Starting generating first {nameof(quantity)} prime numbers");
            var resultArray = new int[quantity + 1];
            var countNumbers = 0;
            for(var i = 2; countNumbers - 1 < quantity; i++)
            {
                if(await _primeNumberChecker.IsPrime(i).ConfigureAwait(false))
                {
                    _logger.LogDebug($"Prime number found: {i}");
                    resultArray[countNumbers] = i;
                    countNumbers++;
                }
            }
            return resultArray;
        }
    }
}
