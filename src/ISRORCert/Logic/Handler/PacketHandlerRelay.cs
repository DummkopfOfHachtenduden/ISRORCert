using ISRORCert.Model;
using ISRORCert.Network;
using ISRORCert.Network.SecurityApi;

using System;
using System.Linq;

namespace ISRORCert.Logic.Handler
{
    internal class PacketHandlerRelay : IPacketHandler
    {
        private readonly PacketHandlerManager _packetHandlerManager;
        private readonly CertificationManager _certificationManager;

        public PacketHandlerRelay(PacketHandlerManager packetHandlerManager, CertificationManager certificationManager)
        {
            _packetHandlerManager = packetHandlerManager;
            _certificationManager = certificationManager;
            _packetHandlerManager[0x6008] = OnRelayMsgReq;
        }

        private bool OnRelayMsgReq(AsyncContext context, Packet packet, int relayID)
        {
            if (_certificationManager.Identity is null)
                return true;

            var realRelayID = packet.ReadInt();
            var realTargetBodyID = packet.ReadShort();
            var realMsgID = packet.ReadUShort();

            if (realTargetBodyID != _certificationManager.Identity.ID)
            {
                // TODO: Relay to the targetBodyID
                return false;
            }

            var realMsg = new Packet(realMsgID, packet.Encrypted, packet.Massive, packet.ReadBytes(packet.RemainingRead()));
            realMsg.Lock();
            return _packetHandlerManager.Handle(context, realMsg, realRelayID);
        }
    }
}
