using ISRORCert.Database;

using System.Data.Common;

namespace ISRORCert.Model
{
    public class Division : IDbEntity
    {
        public byte Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? DBConfig { get; set; } = string.Empty;
        public short ManagerBodyID { get; set; }

        public override string ToString() => $"{Id} [{Name}]";

        public void Read(DbDataReader reader)
        {
            Id = reader.GetByte(0);
            Name = reader.GetString(1);
            DBConfig = reader.GetNullableString(2);
        }
    }
}