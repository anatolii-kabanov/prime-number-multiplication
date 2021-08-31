using System.Threading;
using System.Threading.Tasks;

namespace Avast.PrimeNumber.Services
{
    public interface IPrimeNumberService
    {
        Task<int> RunAsync(string quantity, CancellationToken cancellationToken = default);
    }
}
