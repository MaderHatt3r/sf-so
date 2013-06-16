using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InternalLibrary.IO
{
    public static class ThreadTasks
    {
        private static List<Thread> threads = new List<Thread>();
        private static List<Task> tasks = new List<Task>();
        private static Object taskLock = new Object();

        /// <summary>
        /// Runs the thread.
        /// </summary>
        /// <param name="threadStart">The thread start.</param>
        //public static void RunThread(ThreadStart threadStart)
        //{
        //    threads.Add(new Thread(threadStart));
        //    threads[threads.Count - 1].Start();
        //}

        /// <summary>
        /// Runs the thread.
        /// </summary>
        /// <param name="paramThreadStart">The param thread start.</param>
        /// <param name="param">The param.</param>
        //public static void RunThread(ParameterizedThreadStart paramThreadStart, object param)
        //{
        //    threads.Add(new Thread(paramThreadStart));
        //    threads[threads.Count - 1].Start(param);
        //}

        public static void RunThread(Task newTask)
        {
            lock (taskLock)
            {
                Task run = new Task(() => runThread(newTask));
                run.Start();
            }
        }

        private static void runThread(Task newTask)
        {
            foreach (Task task in tasks)
            {
                task.Wait();
            }
            tasks.Add(newTask);
            newTask.Start();
        }

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

        /// <summary>
        /// Runs the thread.
        /// </summary>
        /// <param name="paramThreadStart">The param thread start.</param>
        /// <param name="parameters">The parameters.</param>
        //public static void RunThread(ParameterizedThreadStart paramThreadStart, List<object>parameters)
        //{

        //}

        /// <summary>
        /// Waits for running threads.
        /// </summary>
        //public static void waitForRunningThreads()
        //{
        //    foreach (Thread thread in threads)
        //    {
        //        if (!thread.ThreadState.Equals(System.Threading.ThreadState.Suspended))
        //        {
        //            thread.Join(10000);
        //        }
        //    }
        //}

        /// <summary>
        /// Resumes the suspended threads.
        /// </summary>
        //public static void resumeSuspendedThreads()
        //{
        //    foreach (Thread thread in threads)
        //    {
        //        if (thread.ThreadState.Equals(System.Threading.ThreadState.Suspended))
        //        {
        //            thread.Resume();
        //        }
        //    }
        //}

        /// <summary>
        /// Aborts the suspended threads.
        /// </summary>
        //public static void abortSuspendedThreads()
        //{
        //    foreach (Thread thread in threads)
        //    {
        //        if (thread.ThreadState.Equals(System.Threading.ThreadState.Suspended))
        //        {
        //            try
        //            {
        //                thread.Abort();
        //            }
        //            catch (ThreadStateException tse)
        //            {
        //                thread.Resume();
        //            }
        //        }
        //    }
        //}



    }
}
