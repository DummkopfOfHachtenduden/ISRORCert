using ISRORCert.Network;
using ISRORCert.Network.SecurityApi;

namespace ISRORCert.Logic.Handler
{
    public delegate bool PacketHandler(AsyncContext context, Packet packet, int relayID);
}
