using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams
{
    public class Stream<T>
    {
        private IEnumerator<T> source;
        private int position = 0;
        private bool atEnd = false;

        public Stream(IEnumerator<T> source)
        {
            this.source = source;
            atEnd = !source.MoveNext();
        }

        public Stream(IEnumerable<T> source) : this(source.GetEnumerator()) { }

        public int Position { get { return position; } }
        public bool AtEnd { get { return atEnd; } }

        public T Next()
        {
            if (AtEnd) return default(T);
            T result = source.Current;
            atEnd = !source.MoveNext();
            position++;
            return result;
        }

        public T Peek()
        {
            if (AtEnd) return default(T);
            return source.Current;
        }

        public IEnumerable<T> Next(int count)
        {
            List<T> result = new List<T>();
            int i = 0;
            while (!AtEnd && count > i++)
            {
                result.Add(Next());
            }
            return result;
        }

        public IEnumerable<T> UpTo(Func<T, bool> condition)
        {
            List<T> result = new List<T>();
            T next;
            while (!AtEnd && !condition(next = Next()))
            {
                result.Add(next);
            }
            return result;
        }
        
        public IEnumerable<T> UpTo(T element)
        {
            return UpTo((each) => Equals(element, each));
        }

        public IEnumerable<T> UpToEnd()
        {
            List<T> result = new List<T>();
            while (!AtEnd)
            {
                result.Add(Next());
            }
            return result;
        }
    }
}
