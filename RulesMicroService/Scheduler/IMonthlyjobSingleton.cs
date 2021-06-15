using System.Threading;
using System.Threading.Tasks;

namespace RulesMicroService.Scheduler
{
    public interface IMonthlyjobSingleton
    {
        public Task DoMonthlyjob(CancellationToken cancellationToken);
    }
}
