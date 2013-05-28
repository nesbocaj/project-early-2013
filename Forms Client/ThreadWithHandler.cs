using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forms_Client.Lib
{
    public class ThreadWithHandler<T> where T : Exception
    {
        public static Thread Create(ParameterizedThreadStart start, Action<T> handler)
        {
            return new Thread(() =>
            {
                try
                {
                    start.Invoke(null);
                }
                catch (T exception)
                {
                    handler(exception);
                }
            });
        }

        public static Thread Create(ParameterizedThreadStart start, Action<T> handler, int maxStackSize)
        {
            return Create(start, handler);
        }

        public static Thread Create(ThreadStart start, Action<T> handler)
        {
            return new Thread(() =>
            {
                try
                {
                    start.Invoke();
                }
                catch (T exception)
                {
                    handler(exception);
                }
            });
        }

        public static Thread Create(ThreadStart start, Action<T> handler, int maxStackSize)
        {
            return Create(start, handler);
        }
    }
}
