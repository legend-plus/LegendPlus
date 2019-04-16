using System;

namespace Packets
{
    public class PlayerPositionPacket : Packet {
        public override short id { get{ return 5;} }
        public override string name { get{ return "Player Position";} }

        public int x;
        public int y;

        public PlayerPositionPacket(int posX, int posY)
        {
            x = posX;
            y = posY;
        }
        public PlayerPositionPacket(byte[] received_data)
        {
            var converter = new MiscUtil.Conversion.BigEndianBitConverter();
            x = (int) converter.ToUInt32(received_data, 0);
            y = (int) converter.ToUInt32(received_data, 4);
        }

        public override byte[] encode()
        {
            var converter = new MiscUtil.Conversion.BigEndianBitConverter();
            byte[] data = new byte[8];
            System.Buffer.BlockCopy(converter.GetBytes((UInt32) x), 0, data, 0, 3);
            System.Buffer.BlockCopy(converter.GetBytes((UInt32) y), 0, data, 4, 7);
            return data;
        }
    }

}