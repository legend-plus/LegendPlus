namespace Packets
{
    public class LoginPacket : Packet {
        public override short id { get{ return -2;} }
        public override string name { get{ return "Login";} }

        public string accessToken;

        public static DataType[] schema = {
            new DataEndingString()
        };

        public LoginPacket(string token)
        {
            accessToken = token;
        }
        public LoginPacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            accessToken = (string) decoded[0];
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] {accessToken});
            return output;
        }
    }

}