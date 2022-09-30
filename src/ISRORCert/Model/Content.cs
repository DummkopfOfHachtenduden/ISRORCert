using ISRORCert.Database;

using System.Data.Common;

namespace ISRORCert.Model
{
    public class Content : IDbEntity
    {
        public byte Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public override string ToString() => $"{Id} [{Name}]";

        public void Read(DbDataReader reader)
        {
            Id = reader.GetByte(0);
            Name = reader.GetString(1);
        }
    }
}