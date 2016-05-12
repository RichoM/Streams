using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams
{
    public class IOStream : Stream<byte>
    {
        public IOStream(System.IO.Stream iostream) : base(GetBytesFrom(iostream)) {}

        private static IEnumerable<byte> GetBytesFrom(System.IO.Stream iostream)
        {
            if (iostream.CanRead)
            {
                int read = iostream.ReadByte();
                while (read != -1)
                {
                    yield return (byte)read;
                    read = iostream.ReadByte();
                }
            }
        }
    }
}
