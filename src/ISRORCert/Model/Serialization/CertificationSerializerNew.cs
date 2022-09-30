using ISRORCert.Network.SecurityApi;

using System;
using System.Linq;

namespace ISRORCert.Model.Serialization
{
    internal class CertificationSerializerNew : ICertificationSerializer
    {
        public void Serialize(Packet packet, Content value)
        {
            packet.WriteByte(value.Id);
            packet.WriteString(value.Name);
        }

        public void Serialize(Packet packet, Module value)
        {
            packet.WriteByte(value.Id);
            packet.WriteString(value.Name);
        }

        public void Serialize(Packet packet, Division value)
        {
            packet.WriteByte(value.Id);
            packet.WriteShort(value.ManagerBodyID);
            packet.WriteString(value.Name);
            packet.WriteString(value.DBConfig ?? "");
        }

        public void Serialize(Packet packet, Farm value)
        {
            packet.WriteByte(value.Id);
            packet.WriteByte(value.DivisionID);
            packet.WriteString(value.Name);
            packet.WriteString(value.DBConfig ?? "");
        }

        public void Serialize(Packet packet, FarmContent value)
        {
            packet.WriteByte(value.FarmID);
            packet.WriteByte(value.ContentID);
        }

        public void Serialize(Packet packet, Shard value)
        {
            packet.WriteShort(value.ID);
            packet.WriteShort(value.MaxUser);
            packet.WriteShort(value.ManageBodyID);
            packet.WriteByte(value.ContentID);
            packet.WriteString(value.Name);
            packet.WriteString(value.DBConfig ?? "");
            packet.WriteString(value.LogDBConfig ?? "");
            packet.WriteByte(value.FarmID);
            packet.WriteByte((byte)value.ShardService);
            packet.WriteShort(value.CurrentUser);
        }

        public void Serialize(Packet packet, ServerMachine value)
        {
            var random = new Random();
            var randomJunkShort = (ushort)(random.Next() % ushort.MaxValue);

            packet.WriteInt(value.Id);
            packet.WriteByte(value.DivisionID ?? default);
            packet.WriteString(value.Name);
            packet.WriteString(value.PublicIP);
            packet.WriteString(value.PrivateIP);
            packet.WriteUShort(0);
            packet.WriteShort(value.ManagerBodyID);
        }

        public void Serialize(Packet packet, ServerBody value)
        {
            var random = new Random();
            //var randomJunkCount = (byte)(random.Next() % 255);
            var randomJunkCount = (byte)0;

            packet.WriteByte(value.DivisionID ?? default);
            packet.WriteByte(value.FarmID ?? default);
            packet.WriteByte(randomJunkCount);
            packet.WriteByte(value.ModuleID);
            packet.WriteShort(value.ID);
            packet.WriteByte(value.ModuleType);

            for (int i = 0; i < (byte)(randomJunkCount % 5); i++)
            {
                var randomJunkByte = (byte)(random.Next() % 2 + 1);
                packet.WriteByte(randomJunkByte);
            }

            packet.WriteShort(value.CertifierID ?? 0);
            packet.WriteShort(value.ListenerPort);
            packet.WriteShort(value.ShardID ?? 0);
            packet.WriteInt(value.MachineID);
            packet.WriteInt((int)value.State);
        }

        public void Serialize(Packet packet, ServerCord value)
        {
            var random = new Random();
            //var randomJunkByte0 = (byte)(random.Next() % 255);
            var randomJunkByte0 = (byte)0;

            packet.WriteInt(value.ID);
            packet.WriteShort(value.OutletID);
            packet.WriteByte(randomJunkByte0);
            packet.WriteShort(value.InletID);
            packet.WriteInt((int)value.State);
            packet.WriteByte((byte)value.BindType);
            packet.WriteInt(value.SessionId);
            if (randomJunkByte0 % 7 == 0)
            {
                var randomJunkByte1 = (byte)(random.Next() % 2 + 1);
                packet.WriteByte(randomJunkByte1);
            }
        }

        public void Serialize(Packet packet, CertificationManager certificationMgr, ServerBody certifiedBody)
        {
            // ServerBodies 
            packet.WriteByte(0);
            packet.WriteByte(1);
            Serialize(packet, certifiedBody);

            foreach (var item in certificationMgr.ServerBodies)
            {
                // Skip the certifiedBody because we already wrote that.
                if (item == certifiedBody)
                    continue;

                // Make sure we only share ServerBodies that have no Division or the certifiedBody's Division.
                if (item.DivisionID.HasValue && item.DivisionID != certifiedBody.DivisionID)
                    continue;

                packet.WriteByte(1);
                Serialize(packet, item);
            }
            packet.WriteByte(2);


            // Module
            packet.WriteByte(0);
            foreach (var item in certificationMgr.Modules)
            {
                packet.WriteByte(1);
                Serialize(packet, item);
            }
            packet.WriteByte(2);

            // Content
            packet.WriteByte(0);
            foreach (var item in certificationMgr.Content)
            {
                packet.WriteByte(1);
                Serialize(packet, item);
            }
            packet.WriteByte(2);

            // Division
            packet.WriteByte(0);
            foreach (var item in certificationMgr.Divisions)
            {
                // Make sure we only share Divisions that belong to the certifiedBody.
                if (item.Id != certifiedBody.DivisionID)
                    continue;

                packet.WriteByte(1);
                Serialize(packet, item);
            }
            packet.WriteByte(2);

            // FarmContent
            packet.WriteByte(0);
            foreach (var item in certificationMgr.FarmContent)
            {
                ArgumentNullException.ThrowIfNull(item.Farm);

                // Make sure we only share FarmContent whos Farm belongs to no Division or the certifiedBody's Division.
                if (item.Farm.DivisionID != 0 && item.Farm.DivisionID != certifiedBody.DivisionID)
                    continue;

                packet.WriteByte(1);
                Serialize(packet, item);
            }
            packet.WriteByte(2);

            // Farm
            packet.WriteByte(0);
            foreach (var item in certificationMgr.Farms)
            {
                // Make sure we only share Farms that have no Division or belong to the certifiedBody's Division.
                if (item.DivisionID != 0 && item.DivisionID != certifiedBody.DivisionID)
                    continue;

                packet.WriteByte(1);
                Serialize(packet, item);
            }
            packet.WriteByte(2);

            // ServerMachine
            packet.WriteByte(0);
            foreach (var item in certificationMgr.ServerMachines)
            {
                // Make sure we only share ServerMachines that have no Division or the certifiedBody's Division.
                if (item.DivisionID.HasValue && item.DivisionID != certifiedBody.DivisionID)
                    continue;

                packet.WriteByte(1);
                Serialize(packet, item);
            }
            packet.WriteByte(2);

            // ServerCords
            packet.WriteByte(0);
            foreach (var item in certificationMgr.ServerCords)
            {
                if (!certificationMgr.TryGetServerBody(item.OutletID, out var outletBody))
                    continue;

                if (!certificationMgr.TryGetServerBody(item.InletID, out var inletBody))
                    continue;

                // Make sure we only share ServerCords whos Out- or Inlet have no Division or the certifiedBody's Division.
                if (outletBody.DivisionID.HasValue && outletBody.DivisionID != certifiedBody.DivisionID
                    && inletBody.DivisionID.HasValue && inletBody.DivisionID != certifiedBody.DivisionID)
                    continue;

                packet.WriteByte(1);
                Serialize(packet, item);
            }
            packet.WriteByte(2);

            // Shard
            packet.WriteByte(0);
            foreach (var item in certificationMgr.Shards)
            {
                ArgumentNullException.ThrowIfNull(item.Farm);

                // Make sure we only share Shards whos Farm belongs to no Division or the certifiedBody's Division.
                if (item.Farm.DivisionID != 0 && item.Farm.DivisionID != certifiedBody.DivisionID)
                    continue;

                packet.WriteByte(1);
                Serialize(packet, item);
            }
            packet.WriteByte(2);
        }
    }
}
