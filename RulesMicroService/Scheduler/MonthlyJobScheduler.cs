using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace RulesMicroService.Scheduler
{
    public class MonthlyJobScheduler : IHostedService
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(MonthlyJobScheduler));
        private IMonthlyjobSingleton singleton;

        public MonthlyJobScheduler(IMonthlyjobSingleton singleton)
        {
            this.singleton = singleton;
        }

        /// <summary>
        /// It will starts the scheduler for run regularly to run monthly job
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _log4net.Info("Scheduler Intiated");
            await singleton.DoMonthlyjob(cancellationToken);
        }

        /// <summary>
        /// It will stop the scheduler 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
