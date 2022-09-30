using ISRORCert.Database;

using System.Data.Common;

namespace ISRORCert.Model
{
    public class FarmContent : IDbEntity
    {
        public int ID { get; set; }
        public byte FarmID { get; set; }
        public byte ContentID { get; set; }

        public Farm? Farm { get; set; }
        public Content? Content { get; set; }

        public void Read(DbDataReader reader)
        {
            ID = reader.GetInt32(0);
            FarmID = reader.GetByte(1);
            ContentID = reader.GetByte(2);
        }
    }
}