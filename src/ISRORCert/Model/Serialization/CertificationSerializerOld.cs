using ISRORCert.Network.SecurityApi;

using System;
using System.Linq;

namespace ISRORCert.Model.Serialization
{
    internal class CertificationSerializerOld : ICertificationSerializer
    {
        public void Serialize(Packet packet, Content value)
        {
            packet.WriteByte(value.Id);
            packet.WriteString(value.Name, 64);
        }

        public void Serialize(Packet packet, Module value)
        {
            packet.WriteByte(value.Id);
            packet.WriteString(value.Name, 64);
        }

        public void Serialize(Packet packet, Division value)
        {
            packet.WriteByte(value.Id);
            packet.WriteString(value.Name, 32);
            packet.WriteString(value.DBConfig ?? "", 256);
            packet.WriteShort(value.ManagerBodyID);
        }

        public void Serialize(Packet packet, Farm value)
        {
            packet.WriteByte(value.Id);
            packet.WriteByte(value.DivisionID);
            packet.WriteString(value.Name, 32);
            packet.WriteString(value.DBConfig ?? "", 256);
        }

        public void Serialize(Packet packet, FarmContent value)
        {
            packet.WriteByte(value.FarmID);
            packet.WriteByte(value.ContentID);
            packet.WriteInt(0); // pFarm
        }

        public void Serialize(Packet packet, Shard value)
        {
            packet.WriteShort(value.ID);
            packet.WriteByte(value.FarmID);
            packet.WriteByte(value.ContentID);
            packet.WriteString(value.Name, 32);
            packet.WriteString(value.DBConfig ?? "", 256);
            packet.WriteString(value.LogDBConfig ?? "", 256);
            packet.WriteShort(value.MaxUser);
            packet.WriteShort(value.ManageBodyID);
            packet.WriteInt(0); // pFarm
            packet.WriteByte(0); // Service
            packet.WriteShort(0); // CurrentUser
        }

        public void Serialize(Packet packet, ServerMachine value)
        {
            packet.WriteInt(value.Id);
            packet.WriteByte(value.DivisionID ?? default);
            packet.WriteString(value.Name, 32);
            packet.WriteString(value.PublicIP, 16);
            packet.WriteString(value.PrivateIP, 16);
            packet.WriteShort(value.ManagerBodyID);
        }

        public void Serialize(Packet packet, ServerBody value)
        {
            packet.WriteShort(value.ID);
            packet.WriteByte(value.DivisionID ?? default);
            packet.WriteByte(value.FarmID ?? default);
            packet.WriteShort(value.ShardID ?? default);
            packet.WriteInt(value.MachineID);
            packet.WriteByte(value.ModuleID);
            packet.WriteByte(value.ModuleType);
            packet.WriteShort(value.CertifierID ?? default);
            packet.WriteShort(value.ListenerPort);
            packet.WriteInt((int)value.State);
            packet.WriteInt(0); // pModule
            packet.WriteInt(0); // pServerMachine
            packet.WriteInt(0); // pDivision
            packet.WriteInt(0); // pFarm
            packet.WriteInt(0); // pShard
        }

        public void Serialize(Packet packet, ServerCord value)
        {
            packet.WriteInt(value.ID);
            packet.WriteShort(value.OutletID);
            packet.WriteShort(value.InletID);
            packet.WriteByte((byte)value.BindType);
            packet.WriteInt((int)value.State); // dwState
            packet.WriteInt(value.SessionId); // dwSessionID
        }

        public void Serialize(Packet packet, CertificationManager certificationMgr, ServerBody certifiedBody)
        {
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

            // ServerBodies
            packet.WriteByte(0);

            // certifiedBody goes first
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
        }
    }
}
