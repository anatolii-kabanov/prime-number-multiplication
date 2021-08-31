using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Avast.PrimeNumber.Builders
{
    public class GridBuilder : IGridBuilder
    {
        private readonly ILogger<GridBuilder> _logger;

        public GridBuilder(ILogger<GridBuilder> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string[,] MultiplicationGrid(int[] numbers)
        {
            var resultGrid = new string[numbers.Length, numbers.Length];

            Parallel.For(0, numbers.Length, (i) => {
                _logger.LogDebug($"Multiplication of {numbers[i]}");
                for(var j = i; j < numbers.Length; j++)
                {
                    var multiplyResult = $"{numbers[i] * numbers[j]}"; // should be checked to overflow 
                    resultGrid[i, j] = multiplyResult;
                    resultGrid[j, i] = multiplyResult;
                }
            });

            return resultGrid;
        }
    }
}
