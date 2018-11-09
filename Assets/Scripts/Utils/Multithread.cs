using System.Collections.Generic;
using System.Threading;

public static class Multithread
{
    public abstract class Async
    {
        public abstract void Run();
        public bool HasResult() { return m_has_result; }
        public void SetResult(bool val) { m_has_result = val; }

        private bool m_has_result = false;
    }

	private class Worker
	{
		public void Run()
		{
            while (m_continue)
            {
                Async task = Multithread.GetTask();
                task.Run();
                task.SetResult(true);
            }
		}

		public void Stop()
		{
			m_continue = false;
		}

		private bool m_continue = true;
	}

    static Multithread()
    {
    }

    private static void ThreadProc()
	{
		Worker worker = new Worker ();
		worker.Run ();
	}

	private static void Init()
	{
		if (m_init)
			return;

        m_threads = new Thread[sc_threads];

        for (int i = 0; i < m_threads.Length; ++i)
		{
			m_threads[i] = new Thread (new ThreadStart (ThreadProc));
			m_threads[i].Start();
		}

		m_init = true;
	}

	public static void Call(Async task)
    {
        Monitor.Enter(m_critical_section);

            Init ();

            task.SetResult(false);
            m_queue.Enqueue(task);
		    m_semaphore.Release ();

        Monitor.Exit(m_critical_section);
    }

	private static Async GetTask()
	{
        m_semaphore.WaitOne ();

        Monitor.Enter(m_critical_section);

            Async task = m_queue.Dequeue();

        Monitor.Exit(m_critical_section);
        return task;
    }

	private const int sc_threads = 8;

	private static bool m_init = false;

	private static Queue<Async> m_queue = new Queue<Async>();
	private static Semaphore m_semaphore = new Semaphore (0, int.MaxValue);
    private static object m_critical_section = new object();
    
	private static Thread[] m_threads;
}
