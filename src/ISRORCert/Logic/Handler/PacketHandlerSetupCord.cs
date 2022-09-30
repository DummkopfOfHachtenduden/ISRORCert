using ISRORCert.Model;
using ISRORCert.Network;
using ISRORCert.Network.SecurityApi;

using System;
using System.Linq;

namespace ISRORCert.Logic.Handler
{
    internal class PacketHandlerSetupCord : IPacketHandler
    {
        private readonly CertificationManager _certificationManager;

        public PacketHandlerSetupCord(PacketHandlerManager packetHandlerManager, CertificationManager certificationManager)
        {
            _certificationManager = certificationManager;

            packetHandlerManager[0x2001] = OnSetupCord;
        }

        private bool OnSetupCord(AsyncContext context, Packet packet, int relayID)
        {
            if (_certificationManager.Identity is null)
                return false;

            context.Security.SetTrusted();

            var setupCord = new Packet(0x2001);
            setupCord.WriteString("Certification");
            setupCord.WriteByte(0);
            context.Security.Send(setupCord);
            return true;
        }
    }
}
