using ISRORCert.Database;

using System.Data.Common;

namespace ISRORCert.Model
{
    public class Farm : IDbEntity
    {
        public byte Id { get; set; }
        public byte DivisionID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? DBConfig { get; set; }

        public Division? Division { get; set; }
        public short ManagerBodyID { get; set; }

        public override string ToString() => $"{Id} [{Name}]";

        public void Read(DbDataReader reader)
        {
            Id = reader.GetByte(0);
            DivisionID = reader.GetByte(1);
            Name = reader.GetString(2);
            DBConfig = reader.GetNullableString(3);
        }
    }
}