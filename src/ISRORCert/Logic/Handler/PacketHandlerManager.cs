using ISRORCert.Network;
using ISRORCert.Network.SecurityApi;

namespace ISRORCert.Logic.Handler
{
    internal class PacketHandlerManager
    {
        private Dictionary<ushort, PacketHandler> _handler = new Dictionary<ushort, PacketHandler>();

        public PacketHandler this[ushort opcode]
        {
            get { return _handler[opcode]; }
            set { _handler[opcode] = value; }
        }

        public PacketHandler? DefaultHandler { get; set; }

        public bool TryRegister(ushort opcode, PacketHandler handler) => _handler.TryAdd(opcode, handler);

        public bool TryUnregister(ushort opcode) => _handler.Remove(opcode);

        public bool Handle(AsyncContext context, Packet packet, int relayID = -1)
        {
            return !_handler.TryGetValue(packet.Opcode, out var handler)
                ? DefaultHandler?.Invoke(context, packet, relayID) ?? true
                : handler?.Invoke(context, packet, relayID) ?? false;
        }
    }
}
