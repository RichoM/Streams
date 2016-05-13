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

        private static IEnumerable<char> AsEnumerable(System.IO.TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                foreach (char c in line)
                {
                    yield return c;
                }
            }
        }

        public new string UpToEnd()
        {
            return new string(base.UpToEnd().ToArray());
        }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
