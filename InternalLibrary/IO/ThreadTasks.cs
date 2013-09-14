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
using InternalLibrary.Data;

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
        /// <param name="operation">The operation.</param>
        public static void RunThread(Action operation)
        {
            lock (taskLock)
            {
                Task run = new Task(() => runThread(operation));
                run.Start();
            }
        }

        /// <summary>
        /// Runs the thread.
        /// </summary>
        /// <param name="operation">The operation.</param>
        private static void runThread(Action operation)
        {
            foreach (Task task in tasks)
            {
                task.Wait();
            }
            Task newTask = new Task(() => operation());
            tasks.Add(newTask);
            newTask.Start();
        }

        /// <summary>
        /// Runs the thread unmanaged.
        /// </summary>
        /// <param name="newTask">The new task.</param>
        public static void RunThreadUnmanaged(Task newTask)
        {
            lock (taskLock)
            {
                tasks.Add(newTask);
            }
            newTask.Start();
        }

        /// <summary>
        /// Actions the protect office object model.
        /// </summary>
        /// <param name="operation">The operation.</param>
        public static void ActionProtectOfficeObjectModel(Action operation)
        {
            FunctionProtectOfficeObjectModel(() => { operation(); return 0; });
        }

        /// <summary>
        /// Functions the protect office object model.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>System.Object.</returns>
        public static object FunctionProtectOfficeObjectModel(Func<object> operation)
        {
            System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();

            while (timer.Elapsed < GlobalApplicationOptions.ThreadTaskTimeout)
            {
                try
                {
                    return operation();
                }
                catch (System.Runtime.InteropServices.COMException)
                {

                }
            }

            try
            {
                return operation();
            }
            catch (System.Runtime.InteropServices.COMException come)
            {
                System.Windows.Forms.MessageBox.Show("A problem occurred uploading the file to Google Drive" + Environment.NewLine +
                    "If the following message indicates that the application is busy, please exit all dialogs and try saving the document again:" + Environment.NewLine + Environment.NewLine +
                come.GetType().ToString() + Environment.NewLine + come.Message);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("A problem occurred uploading the file" + Environment.NewLine +
                    e.GetType().ToString() + Environment.NewLine + e.Message);
            }

            return null;
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
                tasks.Clear();
            }
        }

    }
}
