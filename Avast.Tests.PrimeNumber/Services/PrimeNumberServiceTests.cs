using System;
using System.Threading;
using System.Threading.Tasks;
using Avast.PrimeNumber.Builders;
using Avast.PrimeNumber.Generators;
using Avast.PrimeNumber.Managers;
using Avast.PrimeNumber.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Avast.Tests.PrimeNumber.Services
{
    public class PrimeNumberServiceTests
    {
        private readonly IPrimeNumberService _primeNumberService;

        public PrimeNumberServiceTests()
        {
            var mockedPrimeNumberGenerator = new Mock<IPrimeNumberGenerator>();
            mockedPrimeNumberGenerator.Setup(p => p.Next(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(Array.Empty<int>());
            var mockedGridBuilder = new Mock<IGridBuilder>();
            mockedGridBuilder.Setup(p => p.MultiplicationGrid(It.IsAny<int[]>())).Returns(new string[0, 0]);
            var mockedPrintManager = new Mock<IPrintManager>();
            mockedPrintManager.Setup(p => p.ToConsole(It.IsAny<string[,]>()));
            var mockedLogger = new Mock<ILogger<PrimeNumberService>>();
            _primeNumberService = new PrimeNumberService(mockedPrimeNumberGenerator.Object, mockedGridBuilder.Object, mockedPrintManager.Object, mockedLogger.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RunAsync_Throws_ArgumentNullException(string quantity)
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _primeNumberService.RunAsync(quantity).ConfigureAwait(false));
        }

        [Theory]
        [InlineData("!23")]
        [InlineData("asd")]
        public void RunAsync_Throws_ArgumentException(string quantity)
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _primeNumberService.RunAsync(quantity).ConfigureAwait(false));
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-123")]
        public void RunAsync_Throws_ArgumentOutOfRangeException(string quantity)
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _primeNumberService.RunAsync(quantity).ConfigureAwait(false));
        }

        [Fact]
        public async Task RunAsync_Returns_Zero()
        {
            var result = await _primeNumberService.RunAsync("1").ConfigureAwait(false);
            Assert.Equal(0, result);
        }
    }
}
