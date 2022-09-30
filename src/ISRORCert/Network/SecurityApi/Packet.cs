using System;
using System.Linq;
using System.Text;

namespace ISRORCert.Network.SecurityApi
{
    public class Packet
    {
        private ushort m_opcode;
        private PacketWriter m_writer;
        private PacketReader m_reader;
        private bool m_encrypted;
        private bool m_massive;
        private bool m_locked;
        private byte[] m_reader_bytes;
        private object m_lock;

        public ushort Opcode
        {
            get { return m_opcode; }
        }
        public bool Encrypted
        {
            get { return m_encrypted; }
        }
        public bool Massive
        {
            get { return m_massive; }
        }

        public Packet(Packet rhs)
        {
            lock (rhs.m_lock)
            {
                m_lock = new object();

                m_opcode = rhs.m_opcode;
                m_encrypted = rhs.m_encrypted;
                m_massive = rhs.m_massive;

                m_locked = rhs.m_locked;
                if (!m_locked)
                {
                    m_writer = new PacketWriter();
                    m_reader = null;
                    m_reader_bytes = null;
                    m_writer.Write(rhs.m_writer.GetBytes());
                }
                else
                {
                    m_writer = null;
                    m_reader_bytes = rhs.m_reader_bytes;
                    m_reader = new PacketReader(m_reader_bytes);
                }
            }
        }
        public Packet(ushort opcode)
        {
            m_lock = new object();
            m_opcode = opcode;
            m_encrypted = false;
            m_massive = false;
            m_writer = new PacketWriter();
            m_reader = null;
            m_reader_bytes = null;
        }
        public Packet(ushort opcode, bool encrypted)
        {
            m_lock = new object();
            m_opcode = opcode;
            m_encrypted = encrypted;
            m_massive = false;
            m_writer = new PacketWriter();
            m_reader = null;
            m_reader_bytes = null;
        }
        public Packet(ushort opcode, bool encrypted, bool massive)
        {
            if (encrypted && massive)
            {
                throw new Exception("[Packet::Packet] Packets cannot both be massive and encrypted!");
            }
            m_lock = new object();
            m_opcode = opcode;
            m_encrypted = encrypted;
            m_massive = massive;
            m_writer = new PacketWriter();
            m_reader = null;
            m_reader_bytes = null;
        }
        public Packet(ushort opcode, bool encrypted, bool massive, byte[] bytes)
        {
            if (encrypted && massive)
            {
                throw new Exception("[Packet::Packet] Packets cannot both be massive and encrypted!");
            }
            m_lock = new object();
            m_opcode = opcode;
            m_encrypted = encrypted;
            m_massive = massive;
            m_writer = new PacketWriter();
            m_writer.Write(bytes);
            m_reader = null;
            m_reader_bytes = null;
        }
        public Packet(ushort opcode, bool encrypted, bool massive, byte[] bytes, int offset, int length)
        {
            if (encrypted && massive)
            {
                throw new Exception("[Packet::Packet] Packets cannot both be massive and encrypted!");
            }
            m_lock = new object();
            m_opcode = opcode;
            m_encrypted = encrypted;
            m_massive = massive;
            m_writer = new PacketWriter();
            m_writer.Write(bytes, offset, length);
            m_reader = null;
            m_reader_bytes = null;
        }

        public byte[] GetBytes()
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    return m_reader_bytes;
                }
                return m_writer.GetBytes();
            }
        }

        public void Lock()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    m_reader_bytes = m_writer.GetBytes();
                    m_reader = new PacketReader(m_reader_bytes);
                    m_writer.Close();
                    m_writer = null;
                    m_locked = true;
                }
            }
        }

        public long SeekRead(long offset, SeekOrigin orgin)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot SeekRead on an unlocked Packet.");
                }
                return m_reader.BaseStream.Seek(offset, orgin);
            }
        }

        public int RemainingRead()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot SeekRead on an unlocked Packet.");
                }
                return (int)(m_reader.BaseStream.Length - m_reader.BaseStream.Position);
            }
        }

        public byte ReadByte()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadByte();
            }
        }
        public sbyte ReadSByte()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadSByte();
            }
        }
        public ushort ReadUShort()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadUInt16();
            }
        }
        public short ReadShort()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadInt16();
            }
        }
        public uint ReadUInt()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadUInt32();
            }
        }
        public int ReadInt()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadInt32();
            }
        }
        public ulong ReadULong()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadUInt64();
            }
        }
        public long ReadLong()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadInt64();
            }
        }
        public float ReadSingle()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadSingle();
            }
        }
        public double ReadDouble()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                return m_reader.ReadDouble();
            }
        }
        public string ReadString()
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                var length = m_reader.ReadInt16();
                return ReadString(length);
            }

        }

        public string ReadString(int length)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }
                var buffer = new byte[length];
                m_reader.Read(buffer, 0, length);
                return Encoding.ASCII.GetString(buffer);
            }
        }


        public byte[] ReadBytes(int count)
        {
            lock (m_lock)
            {
                if (!m_locked)
                {
                    throw new Exception("Cannot Read from an unlocked Packet.");
                }

                return m_reader.ReadBytes(count);
            }
        }

        public long SeekWrite(long offset, SeekOrigin orgin)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot SeekWrite on a locked Packet.");
                }
                return m_writer.BaseStream.Seek(offset, orgin);
            }
        }

        public void WriteByte(byte value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteSByte(sbyte value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteUShort(ushort value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteShort(short value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteUInt(uint value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteInt(int value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteULong(ulong value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteLong(long value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteSingle(float value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteDouble(double value)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }
                m_writer.Write(value);
            }
        }
        public void WriteString(string value)
        {
            WriteUShort((ushort)value.Length);
            WriteString(value, value.Length);
        }

        public void WriteString(string value, int length)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }

                var buffer = Encoding.ASCII.GetBytes(value);
                var count = Math.Min(buffer.Length, length);
                m_writer.Write(buffer, 0, count);
                for (int i = count; i < length; i++)
                    m_writer.Write((byte)'\0');
            }
        }
        public void WriteBytes(byte[] buffer)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }

                m_writer.Write(buffer);
            }
        }

        public void WriteBytes(byte[] buffer, int index, int count)
        {
            lock (m_lock)
            {
                if (m_locked)
                {
                    throw new Exception("Cannot Write to a locked Packet.");
                }

                m_writer.Write(buffer, index, count);
            }
        }

    }
}
