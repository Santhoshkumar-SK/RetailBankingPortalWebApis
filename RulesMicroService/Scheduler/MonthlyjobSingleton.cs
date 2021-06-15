
using RulesMicroService.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RulesMicroService.Scheduler
{
    public class MonthlyjobSingleton : IMonthlyjobSingleton
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(MonthlyjobSingleton));
        private IMonthlyjobService service;
        //private int number = 0;

        public MonthlyjobSingleton(IMonthlyjobService service)
        {
            this.service = service;
        }

        /// <summary>
        /// It will intiated by the scheduler daily at midnight if the day is one then it will intiated the monthly job
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DoMonthlyjob(CancellationToken cancellationToken)
        {
            _log4net.Info("Singleton service Started");

            

            while (!cancellationToken.IsCancellationRequested)
            {
                DateTime today = DateTime.Today;

                if(today.Day == 01)  //Checking for the first day of the month
                {
                   await service.toRunMonthlyjob();
                }
                var nextRunTime = DateTime.Today.AddDays(1).AddHours(1);
                var curTime = DateTime.Now;
                var interval = nextRunTime.Subtract(curTime);
                
                await Task.Delay(interval);
            }
        }



        
    }
}
