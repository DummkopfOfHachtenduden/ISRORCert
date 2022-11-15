using ISRORCert.Database;

using System.Data.Common;
using System.Net;

namespace ISRORCert.Model
{
    public class ServerMachine : IDbEntity
    {
        public int Id { get; set; }
        public byte? DivisionID { get; set; }
        public string Name { get; set; } = "";

        public string PublicIP { get; set; } = "";
        public IPAddress? PublicIPAddress { get; set; }

        public string PrivateIP { get; set; } = "";
        public IPAddress? PrivateIPAddress { get; set; }

        public short ManagerBodyID { get; set; }

        public Division? Division { get; set; }

        public void Read(DbDataReader reader)
        {
            Id = reader.GetInt32(0);
            DivisionID = reader.GetNullable<byte>(1);
            Name = reader.GetString(2);
            PublicIP = reader.GetString(3);
            PrivateIP = reader.GetString(4);
        }

        public string GetIP(ServerCordBindType bindType) => bindType == ServerCordBindType.Public ? PublicIP : PrivateIP;

        public IPAddress GetIPAddress(ServerCordBindType bindType) => bindType == ServerCordBindType.Public ? PublicIPAddress : PrivateIPAddress;


    }
}