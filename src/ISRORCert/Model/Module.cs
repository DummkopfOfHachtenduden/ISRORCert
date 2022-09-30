using ISRORCert.Database;

using System.Data.Common;

namespace ISRORCert.Model
{
    public class Module : IDbEntity
    {
        public byte Id { get; private set; }
        public string Name { get; private set; } = string.Empty;

        public override string ToString() => $"{Id} [{Name}]";

        public void Read(DbDataReader reader)
        {
            Id = reader.GetByte(0);
            Name = reader.GetString(1);
        }
    }
}