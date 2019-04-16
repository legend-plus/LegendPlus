using System;

namespace Packets
{
    public class MoveAndFacePacket : Packet {
        public override short id { get{ return -6;} }
        public override string name { get{ return "Move And Face";} }

        public int x;
        public int y;

        public int facing;

        public MoveAndFacePacket(int posX, int posY, int dirFacing)
        {
            x = posX;
            y = posY;
            facing = dirFacing;
        }
        public MoveAndFacePacket(byte[] received_data)
        {
            var converter = new MiscUtil.Conversion.BigEndianBitConverter();
            x = (int) converter.ToUInt32(received_data, 0);
            y = (int) converter.ToUInt32(received_data, 4);
            facing = (int) received_data[8];
        }

        public override byte[] encode()
        {
            var converter = new MiscUtil.Conversion.BigEndianBitConverter();
            byte[] data = new byte[9];
            System.Buffer.BlockCopy(converter.GetBytes((UInt32) x), 0, data, 0, 4);
            System.Buffer.BlockCopy(converter.GetBytes((UInt32) y), 0, data, 4, 4);
            System.Buffer.BlockCopy(converter.GetBytes((UInt32) facing), 3, data, 8, 1);
            return data;
        }
    }

}