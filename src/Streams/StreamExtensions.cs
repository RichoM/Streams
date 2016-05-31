using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Converts a <c>System.IO.TextReader</c> in an <c>IEnumerable<char></c>
        /// </summary>
        /// <param name="reader">The <c>System.IO.TextReader</c> that will be converted.</param>
        /// <returns>An <c>IEnumerable<char></c> that consumes <paramref name="reader"/>.</returns>
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
