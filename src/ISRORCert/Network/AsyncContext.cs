using ISRORCert.Network.SecurityApi;

using System;

namespace ISRORCert.Network
{
    public class AsyncContext
    {
        public AsyncState State { get; init; }
        public Guid Guid { get; }
        public IAsyncInterface Interface { get; set; }
        public Security Security { get; set; }
        public bool Connected { get; set; }

        public AsyncContext()
        {
            Guid = Guid.NewGuid();
            Security = new Security();
            Security.ChangeIdentity("Certification", 0);
            Security.GenerateSecurity(true, true, true);
        }

        public void Disconnect()
        {
            State.Disconnect();
        }

        public void Send(Packet packet)
        {
            Security.Send(packet);
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