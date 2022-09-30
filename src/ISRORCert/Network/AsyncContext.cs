using ISRORCert.Network.SecurityApi;

using System;

namespace ISRORCert.Network
{
    public class AsyncContext
    {
        public AsyncState State { get; set; }
        public Guid Guid { get { return guid; } }
        public IAsyncInterface Interface { get; set; }
        public Security Security { get; set; }
        public bool Connected { get; set; }

        private Guid guid;

        public AsyncContext()
        {
            guid = Guid.NewGuid();
            Security = new Security();
            Security.ChangeIdentity("Certification", 0);
            Security.GenerateSecurity(true, true, true);
        }

        public void Disconnect()
        {
            State.Disconnect();
        }

        public void Send(byte[] buffer)
        {
            State.Write(new AsyncBuffer(buffer));
        }

        public void Send(byte[] buffer, int offset, int count)
        {
            State.Write(new AsyncBuffer(buffer, offset, count));
        }
    }
}