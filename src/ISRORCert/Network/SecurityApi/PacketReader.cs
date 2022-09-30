using System;
using System.Linq;

namespace ISRORCert.Network.SecurityApi
{
    internal class PacketReader : BinaryReader
    {
        private byte[] m_input;

        public PacketReader(byte[] input)
            : base(new MemoryStream(input, false))
        {
            m_input = input;
        }

        public PacketReader(byte[] input, int index, int count)
            : base(new MemoryStream(input, index, count, false))
        {
            m_input = input;
        }
    }
}
