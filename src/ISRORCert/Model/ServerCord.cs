using ISRORCert.Database;

using System.Data.Common;

namespace ISRORCert.Model
{
    public class ServerCord : IDbEntity
    {
        public int ID { get; set; }
        public short OutletID { get; set; }
        public short InletID { get; set; }
        public ServerCordBindType BindType { get; set; }
        public ServerCordState State { get; set; }
        public int SessionId { get; set; }

        public ServerBody? Outlet { get; set; }
        public ServerBody? Inlet { get; set; }

        public override string ToString()
        {
            return $"{ID} [{Outlet?.Module?.Name ?? "Unknown"} --> {Inlet?.Module?.Name ?? "Unknown"}]";
        }

        public void Read(DbDataReader reader)
        {
            ID = reader.GetInt32(0);
            OutletID = reader.GetInt16(1);
            InletID = reader.GetInt16(2);
            BindType = reader.GetFieldValue<ServerCordBindType>(3);
        }
    }
}