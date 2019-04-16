using System;

namespace Packets
{
    public class MovePacket : Packet {
        public override short id { get{ return -5;} }
        public override string name { get{ return "Move";} }

        public int x;
        public int y;

        public MovePacket(int posX, int posY)
        {
            x = posX;
            y = posY;
        }
        public MovePacket(byte[] received_data)
        {
            var converter = new MiscUtil.Conversion.BigEndianBitConverter();
            x = (int) converter.ToUInt32(received_data, 0);
            y = (int) converter.ToUInt32(received_data, 4);
        }

        public override byte[] encode()
        {
            var converter = new MiscUtil.Conversion.BigEndianBitConverter();
            byte[] data = new byte[8];
            System.Buffer.BlockCopy(converter.GetBytes((UInt32) x), 0, data, 0, 4);
            System.Buffer.BlockCopy(converter.GetBytes((UInt32) y), 0, data, 4, 4);
            return data;
        }
    }

}