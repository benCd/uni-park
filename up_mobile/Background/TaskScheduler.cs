﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace Background
{
    /// <summary>
    /// This class allows for tasks to be scheduled in the
    /// background of the app based on the time parameters 
    /// asigned to them. 
    ///
    /// A general rule of thumb is that a task will be executed 
    /// either at the specified time or if that is not possible 
    /// at the earliest convenience.
    /// 
    /// Tasks are scheduled in a priority queue like list, which means 
    /// that every task is added with a scheduled time and a priority. 
    /// If two tasks are executed at the same time the one with the 
    /// higher priority prevails.
    ///  
    /// </summary>
    public static class TaskScheduler
    {
        /// <summary>
        /// Class wrapper for scheduled functions
        /// </summary>
        private class ScheduledFunction : IComparable
        {
            public string Name { set; get; }
            public int Priority { set; get; }
            public DateTime Time { set; get; }
            public Func<int> Func { set; get; }
            public int MaxExecs { set; get; }
            public int ExecCounter { set; get; }

            public ScheduledFunction(string name, int priority, DateTime time, Func<int> func)
            {
                this.Name = name;
                this.Priority = priority;
                this.Time = time;
                this.Func = func;
                this.MaxExecs = -1;
                this.ExecCounter = 0;
            }

            public ScheduledFunction(string name, int priority, DateTime time, Func<int> func, int ExecTimes)
            {
                this.Name = name;
                this.Priority = priority;
                this.Time = time;
                this.Func = func;
                this.MaxExecs = ExecTimes;
                this.ExecCounter = 0;
            }

            /// <summary>
            /// Compares <c>ScheduledFunction</c> objects based on time scheduled 
            /// and the priority.
            /// Note: comparison is done first on time then priority.
            /// </summary>
            /// <param name="obj"> object to compare to</param>
            /// <returns></returns>
            public int CompareTo(object obj)
            {
                if ((obj as ScheduledFunction).Time < this.Time ||
                    (obj as ScheduledFunction).Time == this.Time && (obj as ScheduledFunction).Priority < this.Priority)
                    return -1;
                else if ((obj as ScheduledFunction).Time == this.Time && (obj as ScheduledFunction).Priority == this.Priority)
                    return 0;
                return 1;
            }
        }

        /// <summary>
        /// Tasklist containing <see cref="ScheduledFunction"/> objects
        /// </summary>
        private static LinkedList<ScheduledFunction> taskList = new LinkedList<ScheduledFunction>();

        /// <summary>
        /// Counter, keeping track of the number of <see cref="ScheduledFunction"/>s in the <see cref="taskList"/>
        /// </summary>
        private static int count = 0;

        /// <summary>
        /// Schedules a function for execution
        /// NOTE: name has to be a unique identifier, =>
        /// </summary>
        /// <param name="name">name key of the function for later access</param>
        /// <param name="priority">priority of the function execution</param>
        /// <param name="scheduledTime">time for the function to be scheduled</param>
        /// <param name="function">function to be executed. Has to have signature () => int </param>
        /// <param name="times"></param>
        /// <returns>true if added, false if not</returns>
        public static bool ScheduleFunctionForExecution(string name, int priority, DateTime scheduledTime, Func<int> function, int times = 1)
        {
            var f = new ScheduledFunction(name, priority, scheduledTime, function);
            var n = new ScheduledFunction("", 0, new DateTime(), () => { return 0; });

            var added = false;

            if (count == 0)
            {
                taskList.AddLast(f);
                count++;
                return true;
            }

            foreach (ScheduledFunction sf in taskList)
                if (sf.Name == name)
                    return false;

            foreach(ScheduledFunction sf in taskList)
            {
                // As long as the time/priority are smaller make the traversed
                // element the current element to add before.
                if (sf.CompareTo(f) <= 0)
                {
                    n = sf;
                    added = true;
                }
                    
                else if (taskList.Last.Value == sf)
                {
                    taskList.AddAfter(taskList.Last, f);
                    count++;
                    Debug.Write("Scheduled " + name + " for Execution at " + scheduledTime.ToLongTimeString());
                    return true;
                }
                    
            }

            if(added)
            {
                count++;
                taskList.AddBefore(taskList.Find(n), f);
                Debug.Write("Scheduled " + name + " for Execution at " + scheduledTime.ToLongTimeString());
            }
            

            return added;
        }
        
        
        /// <summary>
        /// Removes function from schedule based on identifier name
        /// </summary>
        /// <param name="name">Name identifier of function to be removed</param>
        /// <returns>true if removed, false if not</returns>
        public static bool RemoveScheduledFunction(string name)
        {
            var r = new ScheduledFunction("", 0, new DateTime(), () => { return -1; });

            foreach(ScheduledFunction sf in taskList)
            {
                if(name == sf.Name)
                {
                    r = sf;
                    break;
                }
            }

            if (taskList.Remove(r))
            {
                count--;
                return true;
            }
            else
                return false;

        }

        /// <summary>
        /// Executes the current schedule
        /// </summary>
        public async static Task ExecuteSchedule(CancellationToken token)
        {
            Debug.Write("ExecuteSchedule is running!!\n\n");
            await Task.Run(async () =>
            {
                while(true)
                {
                    token.ThrowIfCancellationRequested();

                    //Check in 1 second intervals what task is scheduled next
                    await Task.Delay(1000);
                    //Debug.Write("Checking for Tasks to execute!");

                    //Only check when the list is populated to avoid errors
                    if(count > 0)
                    {
                        //Check if the current time of day has passed the scheduled time
                        if(taskList.Last.Value.Time.TimeOfDay.CompareTo(DateTime.Now.TimeOfDay) <= 0)
                        {
                            Debug.Write("Found Task to be run!");
                            var message = new Messages.ExecuteNextTaskMessage();
                            Application.Current.Properties["execution_schedule_running"] = true;
                            /*
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                //Send a message using the MessagingCenter to execute the next Function
                                MessagingCenter.Send<Messages.ExecuteNextTaskMessage>(message, "ExecuteNextTask");
                            });*/
                            ExecuteNextTask();
                        }
                    }

                }

            });
        }

        /// <summary>
        /// Is triggered by the ExecuteSchedule task when the next task is supposed to be run.
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        public async static void ExecuteNextTask()
        {
            //Asynchronously run a scheduled task
            await Task.Run(async () => {

                Debug.Write("Executing Task: " + taskList.Last.Value.Name);
                //TODO: check return value
                taskList.Last.Value.Func();


                //Increment the execution counter
                taskList.Last.Value.ExecCounter++;

                //Check if any more executions are scheduled if so enqueue again
                if (taskList.Last.Value.ExecCounter < taskList.Last.Value.MaxExecs)
                {
                    taskList.AddFirst(taskList.Last);
                    count++;
                }
                //Remove task from end of list
                taskList.Remove(taskList.Last);
                count--;
            });
        }

    }
}
