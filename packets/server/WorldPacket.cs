using System;

namespace Packets
{
    public class WorldPacket : Packet {
        public override short id { get{ return 3;} }
        public override string name { get{ return "World";} }

        public UInt32 height;
        public UInt32 width;

        public int[,] worldData;

        public int[,] bumpData;

        public static DataType[] schema = {
            new DataUInt(),
            new DataUInt(),
            new DataWorld(),
            new DataWorld()
        };

        public WorldPacket()
        {
            
        }
        public WorldPacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            height = (uint) decoded[0];
            width = (uint) decoded[1];
            worldData = (int [,]) decoded[2];
            bumpData = (int [,]) decoded[3];
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] {height, width, worldData, bumpData});
            return output;
        }
    }

}