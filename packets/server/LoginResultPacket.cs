using System;

namespace Packets
{
    public class LoginResultPacket : Packet {
        public override short id { get{ return 2;} }
        public override string name { get{ return "Login Result";} }

        public int responseCode;
        public string userId;

        public static DataType[] schema = {
            new DataByte(),
            new DataEndingString()
        };


        public LoginResultPacket(int code, string user)
        {
            responseCode = code;
            userId = user;
        }
        public LoginResultPacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            responseCode = (int) decoded[0];
            userId = (string) decoded[1];
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] {responseCode, userId});
            return output;
        }
    }

}