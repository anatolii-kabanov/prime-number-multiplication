using System;
namespace Avast.PrimeNumber.Managers
{
    public class PrintManager : IPrintManager
    {
        public PrintManager()
        {
        }

        public void ToConsole(string[,] grid)
        {
            var length = grid.GetLength(0);
            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < length; j ++)
                {
                    Console.Write($"{grid[i, j]} ");
                }
                Console.WriteLine();
            }
        }
    }
}
