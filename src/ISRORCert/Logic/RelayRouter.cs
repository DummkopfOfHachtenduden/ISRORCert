using ISRORCert.Network;
using ISRORCert.Network.SecurityApi;

using System;
using System.Linq;

namespace ISRORCert.Logic
{
    public static class RelayRouter
    {
        public static bool RouteRelayAck(AsyncContext context, Packet realMsg, int relayID)
        {
            var relayAck = new Packet(0xA008, false, realMsg.Massive);

            relayAck.WriteByte(1); // Success
            relayAck.WriteInt(relayID);
            relayAck.WriteUShort(realMsg.Opcode);
            relayAck.WriteBytes(realMsg.GetBytes());

            context.Security.Send(relayAck);
            return true;
        }
    }
}
