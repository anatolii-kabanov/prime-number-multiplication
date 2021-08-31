using System.Threading;
using System.Threading.Tasks;

namespace Avast.PrimeNumber.Generators
{
    public interface IPrimeNumberGenerator
    {
        Task<int[]> Next(int quantity, CancellationToken cancellationToken = default);
    }
}
