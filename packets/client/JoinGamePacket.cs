namespace Packets
{
    public class JoinGamePacket : Packet {
        public override short id { get{ return -3;} }
        public override string name { get{ return "Join Game";} }


        public JoinGamePacket()
        {
        }
        public JoinGamePacket(byte[] received_data)
        {
            
        }

        public override byte[] encode()
        {
            return new byte[] {};
        }
    }

}