namespace Packets
{
    public class LoginPacket : Packet {
        public override short id { get{ return -2;} }
        public override string name { get{ return "Login";} }

        public string accessToken;

        public LoginPacket(string token)
        {
            accessToken = token;
        }
        public LoginPacket(byte[] received_data)
        {
            accessToken = System.Text.Encoding.UTF8.GetString(received_data, 0, received_data.Length);
        }

        public override byte[] encode()
        {
            return System.Text.Encoding.UTF8.GetBytes(accessToken);
        }
    }

}