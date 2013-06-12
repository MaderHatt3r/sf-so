using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SFSO.IO
{
    internal static class ThreadTasks
    {
        private static List<Thread> threads = new List<Thread>();

        /// <summary>
        /// Runs the thread.
        /// </summary>
        /// <param name="threadStart">The thread start.</param>
        internal static void RunThread(ThreadStart threadStart)
        {
            threads.Add(new Thread(threadStart));
            threads[threads.Count - 1].Start();
        }

        /// <summary>
        /// Runs the thread.
        /// </summary>
        /// <param name="paramThreadStart">The param thread start.</param>
        /// <param name="param">The param.</param>
        internal static void RunThread(ParameterizedThreadStart paramThreadStart, object param)
        {
            threads.Add(new Thread(paramThreadStart));
            threads[threads.Count - 1].Start(param);
        }

        /// <summary>
        /// Runs the thread.
        /// </summary>
        /// <param name="paramThreadStart">The param thread start.</param>
        /// <param name="parameters">The parameters.</param>
        internal static void RunThread(ParameterizedThreadStart paramThreadStart, List<object>parameters)
        {

        }

        /// <summary>
        /// Waits for running threads.
        /// </summary>
        internal static void waitForRunningThreads()
        {
            foreach (Thread thread in threads)
            {
                if (!thread.ThreadState.Equals(System.Threading.ThreadState.Suspended))
                {
                    thread.Join(10000);
                }
            }
        }

        /// <summary>
        /// Resumes the suspended threads.
        /// </summary>
        internal static void resumeSuspendedThreads()
        {
            foreach (Thread thread in threads)
            {
                if (thread.ThreadState.Equals(System.Threading.ThreadState.Suspended))
                {
                    thread.Resume();
                }
            }
        }

        /// <summary>
        /// Aborts the suspended threads.
        /// </summary>
        internal static void abortSuspendedThreads()
        {
            foreach (Thread thread in threads)
            {
                if (thread.ThreadState.Equals(System.Threading.ThreadState.Suspended))
                {
                    try
                    {
                        thread.Abort();
                    }
                    catch (ThreadStateException tse)
                    {
                        thread.Resume();
                    }
                }
            }
        }



    }
}
