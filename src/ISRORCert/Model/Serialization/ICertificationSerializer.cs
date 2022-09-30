using ISRORCert.Network.SecurityApi;

using System;
using System.Linq;

namespace ISRORCert.Model.Serialization
{
    internal interface ICertificationSerializer
    {
        public void Serialize(Packet packet, Content value);

        public void Serialize(Packet packet, Module value);

        public void Serialize(Packet packet, Division value);

        public void Serialize(Packet packet, Farm value);
        public void Serialize(Packet packet, FarmContent value);

        public void Serialize(Packet packet, Shard value);

        public void Serialize(Packet packet, ServerMachine value);

        public void Serialize(Packet packet, ServerBody value);

        public void Serialize(Packet packet, ServerCord value);
        void Serialize(Packet certificateAck, CertificationManager certificationManager, ServerBody serverBody);
    }
}
