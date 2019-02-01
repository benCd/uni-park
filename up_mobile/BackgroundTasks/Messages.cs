using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundTasks
{
    /// <summary>
    /// Defines all messages necessary for tasks running in the background.
    /// If a message is not defined here it might not be able to be found 
    /// by all instances of the program. Please refer to the documentation of 
    /// each message to find out, what their purpose is.
    /// </summary>
    public class Messages
    {
        /// <summary>
        /// Message to start the execution schedule in <see cref="TaskScheduler"/>
        /// </summary>
        public class ExecuteScheduleMessage{};
        
        /// <summary>
        /// Message to noify of the cancellation the execution schedule in <see cref="TaskScheduler"/>
        /// </summary>
        public class CancelExecuteScheduleMessage { };

        /// <summary>
        /// Message to start the next queued task in <see cref="TaskScheduler"/>.
        /// 
        /// NOTE: This message is only supposed to be called from <see cref="TaskScheduler.ExecuteSchedule(System.Threading.CancellationToken)"/>
        /// </summary>
        public class ExecuteNextTaskMessage {}
        

    }
}
