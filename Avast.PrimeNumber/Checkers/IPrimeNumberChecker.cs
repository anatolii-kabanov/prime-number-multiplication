using System.Threading;
using System.Threading.Tasks;

namespace Avast.PrimeNumber.Checkers
{
    public interface IPrimeNumberChecker
    {
        Task<bool> IsPrime(long number, CancellationToken cancellationToken = default);
    }
}
