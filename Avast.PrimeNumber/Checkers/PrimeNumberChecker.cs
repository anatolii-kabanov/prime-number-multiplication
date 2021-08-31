using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Avast.PrimeNumber.Checkers
{
    public class PrimeNumberChecker : IPrimeNumberChecker
    {
        private readonly ILogger<PrimeNumberChecker> _logger;

        public PrimeNumberChecker(ILogger<PrimeNumberChecker> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //Could be in parallel
        public async Task<bool> IsPrime(long number, CancellationToken cancellationToken = default)
        {
            if (number < 2)
            {
                _logger.LogDebug($"Number is less than 2 -> {number}");
                return false;
            }

            var sqrt = (long)Math.Floor(Math.Sqrt(number)); 

            for(var i = 2; i <= sqrt; i++)
            {
                if(number % i == 0)
                {
                    return false;
                }
            }
            _logger.LogDebug($"Number is prime -> {number}");
            return true;
        }
    }
}
