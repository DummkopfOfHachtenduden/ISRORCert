namespace ISRORCert.Network
{
    public class AsyncBuffer
    {
        public byte[] Buffer { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }

        public AsyncBuffer(byte[] buffer, int offset, int count)
        {
            Buffer = buffer;
            Offset = offset;
            Count = count;
        }

        public AsyncBuffer(byte[] buffer)
        {
            Buffer = buffer;
            Offset = 0;
            Count = buffer.Length;
        }
    }
}