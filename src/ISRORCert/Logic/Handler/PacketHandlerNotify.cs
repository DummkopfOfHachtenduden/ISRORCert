using ISRORCert.Model;
using ISRORCert.Network;
using ISRORCert.Network.SecurityApi;

using Microsoft.Extensions.Logging;

using System;
using System.Linq;

namespace ISRORCert.Logic.Handler
{
    internal class PacketHandlerNotify : IPacketHandler
    {
        private readonly ILogger _logger;
        private readonly CertificationManager _certificationManager;

        public PacketHandlerNotify(PacketHandlerManager packetHandlerManager, CertificationManager certificationManager, ILogger<PacketHandlerNotify> logger)
        {
            packetHandlerManager[0x2005] = OnNotify;
            packetHandlerManager[0x6005] = OnNotifyReq;
            _certificationManager = certificationManager;
            _logger = logger;
        }



        private bool OnNotify(AsyncContext context, Packet packet, int relayID)
        {
            if (_certificationManager.Identity is null)
                return true;

            var notifyFlag = (StateNotifyFlag)packet.ReadByte();
            if ((notifyFlag & StateNotifyFlag.ServerBody) != 0)
                OnNotifyServerBody(context, packet);

            if ((notifyFlag & StateNotifyFlag.ServerCord) != 0)
                OnNotifyServerCord(context, packet);

            return true;
        }

        private bool OnNotifyReq(AsyncContext context, Packet packet, int relayID)
        {
            if (_certificationManager.Identity is null)
                return true;

            var notifyFlag = (StateNotifyFlag)packet.ReadByte();

            var ack = new Packet(0x2005, false, true);
            ack.WriteByte((byte)notifyFlag);

            if ((notifyFlag & StateNotifyFlag.ServerBody) != 0)
                OnNotifyReqServerBody(context, packet, ack);

            if ((notifyFlag & StateNotifyFlag.ServerCord) != 0)
                OnNotifyReqServerCord(context, packet, ack);

            context.Security.Send(ack);
            return true;
        }

        private void OnNotifyServerBody(AsyncContext context, Packet packet)
        {
            packet.ReadByte();
            while (true)
            {
                var flag = packet.ReadByte();
                if (flag != 1)
                    break;

                var serverBodyID = packet.ReadShort();
                var serverBodyState = (ServerBodyState)packet.ReadInt();

                if (!_certificationManager.TryGetServerBody(serverBodyID, out var serverBody))
                    continue;

                var prevState = serverBody.State;
                if (prevState != serverBodyState)
                {
                    serverBody.State = serverBodyState;
                    _logger.LogInformation($"{nameof(ServerBody)}#{serverBody} is '{serverBody.State}'");
                }
            }
        }
        private void OnNotifyServerCord(AsyncContext context, Packet packet)
        {
            packet.ReadByte();
            while (true)
            {
                var flag = packet.ReadByte();
                if (flag != 1)
                    break;

                var servreCordID = packet.ReadInt();
                var serverCordState = (ServerCordState)packet.ReadInt();

                if (!_certificationManager.TryGetServerCord(servreCordID, out var serverCord))
                    continue;

                var prevState = serverCord.State;
                if (prevState != serverCordState)
                {
                    serverCord.State = serverCordState;
                    _logger.LogInformation($"{nameof(ServerCord)}#{serverCord} is '{serverCord.State}'");
                }
            }
        }



        private void OnNotifyReqServerBody(AsyncContext context, Packet packet, Packet ack)
        {
            packet.ReadByte();
            ack.WriteByte(0);
            while (true)
            {
                var flag = packet.ReadByte();
                if (flag != 1)
                    break;

                var serverBodyID = packet.ReadShort();
                if (!_certificationManager.TryGetServerBody(serverBodyID, out var serverBody))
                    continue;

                ack.WriteByte(1);
                ack.WriteShort(serverBody.ID);
                ack.WriteInt((int)serverBody.State);
            }
            ack.WriteByte(2);
        }
        private void OnNotifyReqServerCord(AsyncContext context, Packet packet, Packet ack)
        {
            packet.ReadByte();
            ack.WriteByte(0);
            while (true)
            {
                var flag = packet.ReadByte();
                if (flag != 1)
                    break;

                var serverCordID = packet.ReadInt();
                if (!_certificationManager.TryGetServerCord(serverCordID, out var serverCord))
                    continue;

                ack.WriteByte(1);
                ack.WriteInt(serverCord.ID);
                ack.WriteInt((int)serverCord.State);
            }
            ack.WriteByte(2);
        }
    }
}
