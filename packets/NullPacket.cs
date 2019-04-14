namespace Packets
{
    public class NullPacket : Packet {
        public override short id { get{ return 0;} }
        public override string name { get{ return "Null";} }

        public byte[] data;

        public NullPacket()
        {
            data = new byte[] {};
        }
        public NullPacket(byte[] received_data)
        {
            data = received_data;
        }

        public override byte[] encode()
        {
            return data;
        }
    }

}