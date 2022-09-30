using ISRORCert.Model;
using ISRORCert.Model.Serialization;
using ISRORCert.Network;
using ISRORCert.Network.SecurityApi;

using Microsoft.Extensions.Logging;

namespace ISRORCert.Logic.Handler
{
    internal class PacketHandlerCertificate : IPacketHandler
    {
        private ICertificationSerializer _certificationSerializer;
        private CertificationManager _certificationManager;
        private ILogger _logger;

        public PacketHandlerCertificate(ILogger<PacketHandlerCertificate> logger, PacketHandlerManager packetHandlerManager, CertificationManager certificationManager, ICertificationSerializer certificationSerializer)
        {
            _logger = logger;
            _certificationManager = certificationManager;
            packetHandlerManager[0x6003] = OnCertificateReq;
            _certificationSerializer = certificationSerializer;
        }

        private bool OnCertificateReq(AsyncContext context, Packet packet, int relayID)
        {
            if (_certificationManager.Identity is null)
                return false;

            var moduleName = packet.ReadString();
            var moduleAddress = packet.ReadString();
            var modulePort = default(ushort);
            if (moduleName is "AgentServer" or "GameServer")
                modulePort = packet.ReadUShort();

            _logger.LogInformation($"Certification request from {moduleAddress}:{modulePort} ({moduleName})");

            if (!_certificationManager.TryGetCertifiableServerBody(moduleName, moduleAddress, modulePort, out var serverBody))
            {
                _logger.LogError($"Cannot certify server body: {moduleAddress}:{modulePort} ({moduleName})");
                return false;
            }

            var certificateAck = new Packet(0xA003, false, true);
            certificateAck.WriteByte(1); // result

#if true
            certificateAck.WriteByte(3);
            for (int i = 0; i < 3; i++)
            {
                certificateAck.WriteInt(4); // payloadSize
                certificateAck.WriteUInt(0xDEADC0DE); // payload
            }

            var buffer = new byte[128];
            certificateAck.WriteInt(128);
            certificateAck.WriteBytes(buffer);
#endif

            _certificationSerializer.Serialize(certificateAck, _certificationManager, serverBody);

            certificateAck.WriteByte(0); // hasSecurityDesc

            context.Security.Send(certificateAck);
            return true;
        }
    }
}
