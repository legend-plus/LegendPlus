namespace Packets
{
    public class ChatPacket : Packet {
        public override short id { get{ return 7;} }
        public override string name { get{ return "Chat";} }

        public static DataType[] schema = {
            new DataString(),
            new DataString(),
            new DataString(),
            new DataFixedString(32),
            new DataInt(),
            new DataInt()
        };

        public string author;
        public string msg;
        public string userId;
        public string uuid;
        public int x;
    
        public int y;

        public ChatPacket(string chat_author, string chat_msg, string chat_userId, string chat_uuid, int chat_x, int chat_y)
        {
            author = chat_author;
            msg = chat_msg;
            userId = chat_userId;
            uuid = chat_uuid;
            x = chat_x;
            y = chat_y;
        }
        public ChatPacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            author = (string) decoded[0];
            msg = (string) decoded[1];
            userId = (string) decoded[2];
            uuid = (string) decoded[3];
            x = (int) decoded[4];
            y = (int) decoded[5];
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] {author, msg, userId, uuid, x, y});
            return output;
        }
    }

}