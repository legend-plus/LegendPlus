namespace Packets
{
    public class RequestWorldPacket : Packet {
        public override short id { get{ return -4;} }
        public override string name { get{ return "Request World";} }


        public RequestWorldPacket()
        {
        }
        public RequestWorldPacket(byte[] received_data)
        {
            
        }

        public override byte[] encode()
        {
            return new byte[] {};
        }
    }

}