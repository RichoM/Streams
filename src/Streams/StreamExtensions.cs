using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams
{
    public static class StreamExtensions
    {
        public static IEnumerable<char> AsEnumerable(this System.IO.TextReader reader)
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
    }
}
