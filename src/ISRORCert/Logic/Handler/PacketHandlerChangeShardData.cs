using ISRORCert.Model;
using ISRORCert.Model.Serialization;
using ISRORCert.Network;
using ISRORCert.Network.SecurityApi;

using System;
using System.Linq;

namespace ISRORCert.Logic.Handler
{
    internal class PacketHandlerChangeShardData : IPacketHandler
    {
        private readonly CertificationManager _certificationManager;

        public PacketHandlerChangeShardData(PacketHandlerManager packetHandlerManager, CertificationManager certificationManager, ICertificationSerializer certificationSerializer)
        {
            _certificationManager = certificationManager;
            packetHandlerManager[0x6310] = OnChangeShardDataReq;
        }

        private bool OnChangeShardDataReq(AsyncContext context, Packet packet, int relayID)
        {
            var shardID = packet.ReadShort();

            if (!_certificationManager.TryGetShard(shardID, out var shard))
                return RouteChangeShardDataFailed(context, relayID);

            var shardDataType = (ShardDataType)packet.ReadByte();
            if (shardDataType == ShardDataType.Name)
            {
                var newName = packet.ReadString();
                if (!_certificationManager.UpdateShardName(shard, newName))
                    return RouteChangeShardDataFailed(context, relayID);

                var ack = new Packet(0xA310);
                ack.WriteByte(1); // result
                ack.WriteShort(shardID);
                ack.WriteByte((byte)shardDataType);
                ack.WriteString(newName);
                return RelayRouter.RouteRelayAck(context, ack, relayID);
            }
            else if (shardDataType == ShardDataType.MaxUser)
            {
                var newMaxUser = packet.ReadShort();
                if (!_certificationManager.UpdateShardMaxUser(shard, newMaxUser))
                    return RouteChangeShardDataFailed(context, relayID);

                var ack = new Packet(0xA310);
                ack.WriteByte(1); // result
                ack.WriteShort(shardID);
                ack.WriteByte((byte)shardDataType);
                ack.WriteShort(newMaxUser);
                return RelayRouter.RouteRelayAck(context, ack, relayID);
            }
            return RouteChangeShardDataFailed(context, relayID);
        }

        private bool RouteChangeShardDataFailed(AsyncContext context, int relayID)
        {
            var ack = new Packet(0xA310);
            ack.WriteByte(2); // result
            return RelayRouter.RouteRelayAck(context, ack, relayID);
        }
    }
}
