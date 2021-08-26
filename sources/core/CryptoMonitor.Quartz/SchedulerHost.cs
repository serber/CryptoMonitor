using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace CryptoMonitor.Quartz
{
    public class SchedulerHost : IHostedService
    {
        private IScheduler _scheduler;

        private readonly ISchedulerFactory _schedulerFactory;

        private readonly SchedulerOptions _schedulerOptions;

        private readonly ILogger<SchedulerHost> _logger;

        public SchedulerHost(ISchedulerFactory schedulerFactory, IOptions<SchedulerOptions> options, ILogger<SchedulerHost> logger)
        {
            _schedulerFactory = schedulerFactory;
            _schedulerOptions = options.Value;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            await _scheduler.Start(cancellationToken);

            if (_schedulerOptions.Jobs?.Length > 0)
            {
                foreach (var job in _schedulerOptions.Jobs.Where(x => x.Enabled))
                {
                    var jobType = Type.GetType(job.Type);

                    if (jobType == null)
                    {
                        _logger.LogWarning($"Unkown job type: '{job.Type}'");
                        continue;
                    }

                    var jobBuilder = JobBuilder.Create(jobType);
                    if (job.Data != null && job.Data.Count > 0)
                    {
                        jobBuilder = jobBuilder.SetJobData(new JobDataMap((IDictionary<string, object>)job.Data));
                    }

                    var jobDetail = jobBuilder.Build();
                    var trigger = job.Cron.Equals("NOW", StringComparison.OrdinalIgnoreCase)
                        ? TriggerBuilder.Create()
                            .WithIdentity(job.Name, "default")
                            .StartNow()
                            .Build()
                        : TriggerBuilder.Create()
                            .WithIdentity(job.Name, "default")
                            .WithCronSchedule(job.Cron)
                            .Build();

                    await _scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);

                    _logger.LogInformation($"Scheduled job '{job.Name}'");
                }
            }

            _logger.LogInformation($"{nameof(SchedulerHost)} started");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler.Shutdown(cancellationToken);

            _logger.LogInformation($"{nameof(SchedulerHost)} stopped");
        }
    }
}