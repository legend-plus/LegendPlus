namespace Packets
{
    public class PingPacket : Packet {
        public override short id { get{ return -1;} }
        public override string name { get{ return "Ping";} }

        public string message;

        public PingPacket(string msg)
        {
            message = msg;
        }
        public PingPacket(byte[] received_data)
        {
            message = System.Text.Encoding.UTF8.GetString(received_data, 0, received_data.Length);
        }

        public override byte[] encode()
        {
            return System.Text.Encoding.UTF8.GetBytes(message);
        }
    }

}