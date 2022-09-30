using System;
using System.Linq;

namespace ISRORCert.Network.SecurityApi
{
    internal class PacketWriter : BinaryWriter
    {
        private MemoryStream m_ms;

        public PacketWriter()
        {
            m_ms = new MemoryStream();
            OutStream = m_ms;
        }

        public byte[] GetBytes()
        {
            return m_ms.ToArray();
        }
    }
}
