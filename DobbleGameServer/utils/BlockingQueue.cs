using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DobbleGameServer
{
    public class BlockingQueue<T> : Queue<T>
    {
        private Queue<T> queue;

        public BlockingQueue()
        {
            this.queue = new Queue<T>();
            
        }

        
        public new void Enqueue(T item)
        {
            lock (queue)
            {
                queue.Enqueue(item);
                if (queue.Count == 1)
                {
                    // wake up any blocked dequeue
                    Monitor.PulseAll(queue);
                }
            }
        }

        public new T Dequeue()
        {
            while (queue.Count == 0)
            {
                Monitor.Wait(queue);
            }

            return queue.Dequeue();
        }

        public new void Clear()
        {
            queue.Clear();
        }
    }
}
