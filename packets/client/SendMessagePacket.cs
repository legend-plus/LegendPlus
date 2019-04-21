namespace Packets
{
    public class SendMessagePacket : Packet {
        public override short id { get{ return -7;} }
        public override string name { get{ return "Send Message";} }

        public string message;

        public static DataType[] schema = {
            new DataEndingString()
        };

        public SendMessagePacket(string msg)
        {
            message = msg;
        }
        public SendMessagePacket(byte[] received_data)
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