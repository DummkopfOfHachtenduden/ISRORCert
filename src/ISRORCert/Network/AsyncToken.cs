using System.Net.Sockets;

namespace ISRORCert.Network
{
    public class AsyncToken
    {
        public Socket Socket { get; set; }
        public IAsyncInterface Interface { get; set; }
    }
}