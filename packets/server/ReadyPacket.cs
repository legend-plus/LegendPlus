using System;

namespace Packets
{
    public class ReadyPacket : Packet {
        public override short id { get{ return 4;} }
        public override string name { get{ return "Ready";} }

        public int code;

        public ReadyPacket(int readyCode)
        {
            code = readyCode;
        }
        public ReadyPacket(byte[] received_data)
        {
            code = received_data[0];
        }

        public override byte[] encode()
        {
            return new byte[] {BitConverter.GetBytes(code)[0]};
        }
    }

}