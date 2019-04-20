using System;

namespace Packets
{
    public class MoveAndFacePacket : Packet {
        public override short id { get{ return -6;} }
        public override string name { get{ return "Move And Face";} }

        public int x;
        public int y;

        public int facing;

        public static DataType[] schema = {
            new DataInt(),
            new DataInt(),
            new DataByte()
        };

        public MoveAndFacePacket(int posX, int posY, int dirFacing)
        {
            x = posX;
            y = posY;
            facing = dirFacing;
        }
        public MoveAndFacePacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            x = (int) decoded[0];
            y = (int) decoded[1];
            facing = (int) decoded[2];
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] {x, y, facing});
            return output;
        }
    }

}