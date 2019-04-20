namespace Packets
{
    public class PongPacket : Packet {
        public override short id { get{ return 1;} }
        public override string name { get{ return "Pong";} }

        public string message;

        public static DataType[] schema = {
            new DataEndingString()
        };


        public PongPacket(string msg)
        {
            message = msg;
        }
        public PongPacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            message = (string) decoded[0];
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] {message});
            return output;
        }
    }

}