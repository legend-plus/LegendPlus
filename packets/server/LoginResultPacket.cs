using System;

namespace Packets
{
    public class LoginResultPacket : Packet {
        public override short id { get{ return 2;} }
        public override string name { get{ return "Login Result";} }

        public int responseCode;
        public string userId;

        public LoginResultPacket(int code, string user)
        {
            responseCode = code;
            userId = user;
        }
        public LoginResultPacket(byte[] received_data)
        {
            responseCode = received_data[0];
            userId = System.Text.Encoding.UTF8.GetString(received_data, 1, received_data.Length - 1);
        }

        public override byte[] encode()
        {
            var responseCodeByte = BitConverter.GetBytes(responseCode)[0];
            var userIdBytes = System.Text.Encoding.UTF8.GetBytes(userId);
            var output = new byte[1 + userIdBytes.Length];

            System.Buffer.SetByte(output, 0, responseCodeByte);
            System.Buffer.BlockCopy(userIdBytes, 0, output, 1, userIdBytes.Length);

            return output;
        }
    }

}