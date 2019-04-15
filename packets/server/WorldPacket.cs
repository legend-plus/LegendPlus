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
        public WorldPacket()
        {
            
        }
        public WorldPacket(byte[] received_data)
        {
            var converter = new MiscUtil.Conversion.BigEndianBitConverter();
            height = converter.ToUInt32(received_data, 0);
            width = converter.ToUInt32(received_data, 4);
            var worldLength = (int) converter.ToUInt32(received_data, 8);
            Godot.GD.Print(worldLength);
            var worldWordSize = (int) converter.ToUInt32(received_data, 12+worldLength);

            var bumpLength = (int) converter.ToUInt32(received_data, 16+worldLength);
            var bumpWordSize = (int) converter.ToUInt32(received_data, 20+worldLength+bumpLength);

            worldData = new int[height, width];

            int delta = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int cell = 0;
                    switch (worldWordSize)
                    {
                        case 1:
                            cell = received_data[12+delta];
                            break;
                        case 2:
                            cell = converter.ToUInt16(received_data, 12+delta);
                            break;
                        default:
                            cell = (int) converter.ToUInt32(received_data, 12+delta);
                            break;
                    }
                    worldData[y, x] = cell;
                    delta += worldWordSize;
                }
            }

            bumpData = new int[height, width];

            delta = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int cell = 0;
                    switch (bumpWordSize)
                    {
                        case 1:
                            cell = received_data[20+worldLength+delta];
                            break;
                        case 2:
                            cell = converter.ToUInt16(received_data, 20+worldLength+delta);
                            break;
                        default:
                            cell = (int) converter.ToUInt32(received_data, 20+worldLength+delta);
                            break;
                    }
                    bumpData[y, x] = cell;
                    delta += bumpWordSize;
                }
            }
        }

        public override byte[] encode()
        {
            return new byte[] {};
        }
    }

}