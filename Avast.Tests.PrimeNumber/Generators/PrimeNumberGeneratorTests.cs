using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avast.PrimeNumber.Checkers;
using Avast.PrimeNumber.Generators;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Avast.Tests.PrimeNumber.Generators
{
    public class PrimeNumberGeneratorTests
    {
        private readonly IPrimeNumberGenerator _primeNumberGenerator;
        private readonly long[] _primeNumbersArray = new long[] { 2, 3, 5, 7, 11, 13 };

        public PrimeNumberGeneratorTests()
        {
            var mockedLogger = new Mock<ILogger<PrimeNumberGenerator>>();
            var mockedPrimeNumberChecker = new Mock<IPrimeNumberChecker>();
            mockedPrimeNumberChecker.Setup(p => p.IsPrime(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _primeNumberGenerator = new PrimeNumberGenerator(mockedPrimeNumberChecker.Object, mockedLogger.Object);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Next_Should_Return_Excpected_Array(int quantity)
        {
            var resultArray = await _primeNumberGenerator.Next(quantity).ConfigureAwait(false);

            Assert.Equal(resultArray.Length, quantity + 1);
            Assert.All(resultArray, item => _primeNumbersArray.Contains(item));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-123)]
        public void Next_Throws_ArgumentOutOfRangeException(int quantity)
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _primeNumberGenerator.Next(quantity).ConfigureAwait(false));
        }
    }
}
