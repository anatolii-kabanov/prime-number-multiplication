using System;
using Avast.PrimeNumber.Builders;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Avast.Tests.PrimeNumber.Builders
{
    public class GridBuilderTests
    {
        private readonly IGridBuilder _gridBuilder;
        public GridBuilderTests()
        {
            var mockedLogger = new Mock<ILogger<GridBuilder>>();
            _gridBuilder = new GridBuilder(mockedLogger.Object);
        }

        [Theory]
        [InlineData(new[] {2, 3})]
        [InlineData(new[] {2, 3, 4})]
        public void Grid_Should_Value(int[] numbers)
        {
            var result = _gridBuilder.MultiplicationGrid(numbers);

            for(var i = 0; i < numbers.Length; i++)
            {
                Assert.Equal(int.Parse(result[i, i]), numbers[i] * numbers[i]);
            }
        }

        [Theory]
        [InlineData(null)]
        public void Grid_Should_Throw_ArgumentNullException(int[] numbers)
        {
            Assert.Throws<ArgumentNullException>(() => _gridBuilder.MultiplicationGrid(numbers));
        }
    }
}
