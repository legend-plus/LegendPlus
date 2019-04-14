namespace Packets
{
    public class ReadyPacket : Packet {
        public override short id { get{ return 4;} }
        public override string name { get{ return "Ready";} }

        public byte[] data;

        public ReadyPacket()
        {
            data = new byte[] {};
        }
        public ReadyPacket(byte[] received_data)
        {
            data = received_data;
        }

        public override byte[] encode()
        {
            return data;
        }
    }

}