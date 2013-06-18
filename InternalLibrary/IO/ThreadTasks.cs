// ***********************************************************************
// Assembly         : InternalLibrary
// Author           : CTDragon
// Created          : 06-13-2013
//
// Last Modified By : CTDragon
// Last Modified On : 06-16-2013
// ***********************************************************************
// <copyright file="ThreadTasks.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InternalLibrary.IO
{
    /// <summary>
    /// Class ThreadTasks
    /// </summary>
    public static class ThreadTasks
    {
        /// <summary>
        /// The tasks
        /// </summary>
        private static List<Task> tasks = new List<Task>();
        /// <summary>
        /// The task lock
        /// </summary>
        private static Object taskLock = new Object();

        /// <summary>
        /// Runs the thread.
        /// </summary>
        /// <param name="newTask">The new task.</param>
        public static void RunThread(Task newTask)
        {
            lock (taskLock)
            {
                Task run = new Task(() => runThread(newTask));
                run.Start();
            }
        }

        public static void RunThreadUnmanaged(Task newTask)
        {
            lock (taskLock)
            {
                tasks.Add(newTask);
            }
            newTask.Start();
        }

        /// <summary>
        /// Runs the thread.
        /// </summary>
        /// <param name="newTask">The new task.</param>
        private static void runThread(Task newTask)
        {
            foreach (Task task in tasks)
            {
                task.Wait();
            }
            tasks.Add(newTask);
            newTask.Start();
        }

        /// <summary>
        /// Waits for running tasks.
        /// </summary>
        public static void WaitForRunningTasks()
        {
            lock (taskLock)
            {
                foreach (Task task in tasks)
                {
                    task.Wait();
                }
            }
        }

    }
}
