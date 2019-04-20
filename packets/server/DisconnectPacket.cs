namespace Packets
{
    public class DisconnectPacket : Packet {
        public override short id { get{ return 6;} }
        public override string name { get{ return "Disconnect";} }

        public string message;

        public static DataType[] schema = {
            new DataEndingString()
        };

        public DisconnectPacket(string msg)
        {
            message = msg;
        }
        public DisconnectPacket(byte[] received_data)
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