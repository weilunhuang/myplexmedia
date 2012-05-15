using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PlexMediaCenter.Util {
    public class BlockingQueue<T> : IDisposable {
        private Queue<T> _queue;
        private Semaphore _semaphore;

        public BlockingQueue(int maxThreads) {
            _queue = new Queue<T>();
            _semaphore = new Semaphore(0, maxThreads);
        }

        public void Reset() {
            lock (_queue) _queue.Clear();
        }

        public void Enqueue(T data) {
            if (data == null) throw new ArgumentNullException("data");
            lock (_queue) _queue.Enqueue(data);
            _semaphore.Release();
        }

        public T Dequeue() {
            _semaphore.WaitOne();
            lock (_queue) {
                try { return _queue.Dequeue(); } catch {
                    return default(T);
                }
            }
        }

        public IEnumerator<T> GetEnumerator() {
            while (true) yield return Dequeue();
        }

        public void Dispose() {
            if (_semaphore != null) {
                _semaphore.Close();
                _semaphore = null;
            }
        }
    }
}
