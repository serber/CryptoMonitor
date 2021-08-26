using System.Collections.Generic;

namespace CryptoMonitor.Quartz
{
    /// <summary>
    /// Shceduler options
    /// </summary>
    public class SchedulerOptions
    {
        /// <summary>
        /// Jobs array
        /// </summary>
        public Job[] Jobs { get; set; }

        /// <summary>
        /// Scheduler job
        /// </summary>
        public class Job
        {
            /// <summary>
            /// Job short name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Cron expression
            /// </summary>
            public string Cron { get; set; }

            /// <summary>
            /// Job class type
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// Enabled or not
            /// </summary>
            public bool Enabled { get; set; }

            /// <summary>
            /// Job additional data
            /// </summary>
            public Dictionary<string, object> Data { get; set; }
        }
    }
}