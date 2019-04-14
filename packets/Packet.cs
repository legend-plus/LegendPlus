
namespace Packets
{
    public abstract class Packet {
        public abstract short id { get; }
        public abstract string name { get; }

        public abstract byte[] encode();
}
}
