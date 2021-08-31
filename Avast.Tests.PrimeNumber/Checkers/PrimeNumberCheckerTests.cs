using System.Threading;
using System.Threading.Tasks;
using Avast.PrimeNumber.Checkers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Avast.Tests.PrimeNumber.Checkers
{
    public class PrimeNumberCheckerTests
    {
        private readonly IPrimeNumberChecker _primeNumberChecker;
        public PrimeNumberCheckerTests()
        {
            var mockedLogger = new Mock<ILogger<PrimeNumberChecker>>();
            _primeNumberChecker = new PrimeNumberChecker(mockedLogger.Object);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(7)]
        public async Task IsPrime_Should_Return_True(int number)
        {
            var isPrime = await _primeNumberChecker.IsPrime(number).ConfigureAwait(false);

            Assert.True(isPrime);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(8)]
        public async Task IsPrime_Should_Return_False(int number)
        {
            var isPrime = await _primeNumberChecker.IsPrime(number).ConfigureAwait(false);

            Assert.False(isPrime);
        }
    }
}
