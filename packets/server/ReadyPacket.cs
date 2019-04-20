using System;

namespace Packets
{
    public class ReadyPacket : Packet {
        public override short id { get{ return 4;} }
        public override string name { get{ return "Ready";} }

        public int code;

        public static DataType[] schema = {
            new DataByte()
        };

        public ReadyPacket(int readyCode)
        {
            code = readyCode;
        }
        public ReadyPacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            code = (int) decoded[0];
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] {code});
            return output;
        }
    }

}