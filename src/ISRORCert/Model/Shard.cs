using ISRORCert.Database;

using System.Data.Common;

namespace ISRORCert.Model
{
    public class Shard : IDbEntity
    {
        public short ID { get; set; }
        public byte FarmID { get; set; }
        public byte ContentID { get; set; }
        public string Name { get; set; } = "";
        public string? DBConfig { get; set; }
        public string? LogDBConfig { get; set; }
        public short MaxUser { get; set; }
        public short ManageBodyID { get; set; }
        public ShardService ShardService { get; set; }
        public short CurrentUser { get; set; }

        public Farm? Farm { get; set; }
        public Content? Content { get; set; }

        public void Read(DbDataReader reader)
        {
            ID = reader.GetInt16(0);
            FarmID = reader.GetByte(1);
            ContentID = reader.GetByte(2);
            Name = reader.GetString(3);
            DBConfig = reader.GetNullableString(4);
            LogDBConfig = reader.GetNullableString(5);
            MaxUser = reader.GetInt16(6);
        }

        public override string ToString() => $"{ID} ({Name})";
    }
}