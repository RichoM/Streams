using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams
{
    /// <summary>
    /// A <c>Stream</c> is a sequence of objects, similar to an <c>IEnumerator</c>. The big difference is that while the 
    /// enumerator points to an element, a stream contains a pointer between elements. Also, the interface differs quite a bit.
    /// </summary>
    /// <typeparam name="T">The type of elements this stream will contain.</typeparam>
    public class Stream<T> : IDisposable
    {
        private IEnumerator<T> source;
        private int position = -1;
        private bool atEnd = false;

        /// <summary>
        /// Creates a <c>Stream</c> by wrapping an <c>IEnumerator</c>.
        /// </summary>
        /// <param name="source">The enumerator to wrap.</param>
        public Stream(IEnumerator<T> source)
        {
            this.source = source;
            Advance();
        }

        /// <summary>
        /// Creates a <c>Stream</c> by wrapping an <c>IEnumerable</c>.
        /// </summary>
        /// <param name="source">The enumerable to wrap.</param>
        public Stream(IEnumerable<T> source) : this(source.GetEnumerator()) { }

        /// <summary>
        /// Returns the position of this stream.
        /// This number represents how many elements we retrieved already.
        /// </summary>
        public int Position { get { return position; } }

        /// <summary>
        /// Returns <c>true</c> if all the elements from the stream have been retrieved.
        /// </summary>
        public bool AtEnd { get { return atEnd; } }

        /// <summary>
        /// Returns the next element (if any) and advances the stream.
        /// </summary>
        /// <returns>The next element on the stream. If the stream is at end
        /// it returns <c>null</c> (or the default value, if the stream contains value types)</returns>
        public T Next()
        {
            if (AtEnd) return default(T);
            T result = source.Current;
            Advance();
            return result;
        }

        /// <summary>
        /// Returns the next element (if any) without advancing stream.
        /// </summary>
        /// <returns>The next element on the stream. If the stream is at end
        /// it returns <c>null</c> (or the default value, if the stream contains value types)</returns>
        public T Peek()
        {
            if (AtEnd) return default(T);
            return source.Current;
        }

        /// <summary>
        /// Advances the stream by one element.
        /// </summary>
        public void Skip()
        {
            if (AtEnd) return;
            Advance();
        }

        /// <summary>
        /// Advances the stream by <paramref name="count"/>.
        /// </summary>
        /// <param name="count">How many elements to skip.</param>
        public void Skip(int count)
        {
            for (int i = 0; i < count; i++) { Skip(); }
        }

        /// <summary>
        /// Returns the next <paramref name="count"/> elements.
        /// It also advances the stream by <paramref name="count"/>.
        /// </summary>
        /// <param name="count">How many elements to return.</param>
        /// <returns>An IEnumerable containing as many elements as the <paramref name="count"/>
        /// indicates (or less, if the stream reaches the end).
        /// </returns>
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

        /// <summary>
        /// Returns all the elements from the current position until the <paramref name="condition"/>
        /// evaluates to <c>true</c>. 
        /// </summary>
        /// <param name="condition">The condition that will be checked for each element.</param>
        /// <returns>An IEnumerable containing all the elements from the current position to the element
        /// (not inclusive) that mets the <paramref name="condition"/>. If the condition is not met by
        /// any element, this method returns all the elements until the end of the stream.
        /// </returns>
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
        
        /// <summary>
        /// Returns all the elements from the current position to the occurrence (if any, but
        /// not inclusive) of <paramref name="element"/> in the stream.
        /// </summary>
        /// <param name="element">The element that will be looked in the stream.</param>
        /// <returns>An IEnumerable containing all the elements from the current position to the
        /// occurrence (if any, but not inclusive) of <paramref name="element"/>. If no occurrence
        /// is found, the method returns all the elements until the end of the stream.</returns>
        public IEnumerable<T> UpTo(T element)
        {
            return UpTo((each) => Equals(element, each));
        }

        /// <summary>
        /// Returns all the elements until the end of the stream.
        /// </summary>
        /// <returns>An <c>IEnumerable</c> containing all the elements until the end.</returns>
        public IEnumerable<T> UpToEnd()
        {
            List<T> result = new List<T>();
            while (!AtEnd)
            {
                result.Add(Next());
            }
            return result;
        }

        private void Advance()
        {
            atEnd = !source.MoveNext();
            position++;
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                source.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
