using System;

namespace Packets
{
    public class PlayerPositionPacket : Packet {
        public override short id { get{ return 5;} }
        public override string name { get{ return "Player Position";} }

        public int x;
        public int y;

        public static DataType[] schema = {
            new DataInt(),
            new DataInt()
        };

        public PlayerPositionPacket(int posX, int posY)
        {
            x = posX;
            y = posY;
        }
        public PlayerPositionPacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            x = (int) decoded[0];
            y = (int) decoded[1];
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] {x, y});
            return output;
        }
    }

}