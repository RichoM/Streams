using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams
{
    public class TextStream : Stream<char>, IDisposable
    {
        private System.IO.TextReader reader;

        public TextStream(System.IO.TextReader reader) : base(AsEnumerable(reader))
        {
            this.reader = reader;
        }

        public TextStream(System.IO.Stream iostream, Encoding encoding = null)
            : this(new System.IO.StreamReader(iostream, encoding ?? Encoding.Default))
        {}

        public TextStream(string source) : this(new System.IO.StringReader(source))
        {}

        private static IEnumerable<char> AsEnumerable(System.IO.TextReader reader)
        {
            char[] buffer = new char[4096];
            bool atEnd = false;
            while (!atEnd)
            {
                int charsRead = reader.Read(buffer, 0, buffer.Length);
                if (charsRead <= 0)
                {
                    atEnd = true;
                }
                else
                {
                    for (int i = 0; i < charsRead; i++)
                    {
                        yield return buffer[i];
                    }
                }
            }
        }

        public new string UpToEnd()
        {
            return new string(base.UpToEnd().ToArray());
        }
        
        public new string UpTo(Func<char, bool> condition)
        {
            StringBuilder sb = new StringBuilder();
            char next;
            while (!AtEnd && !condition(next = Next()))
            {
                sb.Append(next);
            }
            return sb.ToString();
        }

        public new string UpTo(char element)
        {
            return UpTo((each) => Equals(element, each));
        }

        public new string Next(int count)
        {
            return new string(base.Next(count).ToArray());
        }

        public void Dispose()
        {
            if (reader != null) { reader.Dispose(); }
        }
    }
}
