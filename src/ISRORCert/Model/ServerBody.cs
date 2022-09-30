using ISRORCert.Database;

using System.Data.Common;

namespace ISRORCert.Model
{
    public class ServerBody : IDbEntity
    {
        public short ID { get; set; }
        public byte? DivisionID { get; set; }
        public byte? FarmID { get; set; }
        public short? ShardID { get; set; }
        public int MachineID { get; set; }
        public byte ModuleID { get; set; }
        public byte ModuleType { get; set; }
        public short? CertifierID { get; set; }
        public short ListenerPort { get; set; }
        public ServerBodyState State { get; set; }

        public Division? Division { get; set; }
        public Farm? Farm { get; set; }
        public Shard? Shard { get; set; }
        public ServerMachine? Machine { get; set; }
        public Module? Module { get; set; }
        public ServerBody? Certifier { get; set; }

        public override string ToString()
        {
            return $"{ID} - {Module?.Name ?? "<Unknown>"}";
        }

        public void Read(DbDataReader reader)
        {
            ID = reader.GetInt16(0);
            DivisionID = reader.GetNullable<byte>(1);
            FarmID = reader.GetNullable<byte>(2);
            ShardID = reader.GetNullable<short>(3);
            MachineID = reader.GetInt32(4);
            ModuleID = reader.GetByte(5);
            ModuleType = reader.GetByte(6);
            CertifierID = reader.GetNullable<short>(7);
            ListenerPort = reader.GetInt16(8);
        }
    }
}