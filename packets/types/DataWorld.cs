using MiscUtil.Conversion;

namespace Packets
{
    class DataWorld : DataVarying
    {
        public override int getLength(byte[] data, MiscUtil.Conversion.BigEndianBitConverter converter, int offset = 0)
        {
            var worldLength = (int) converter.ToUInt32(data, offset+8);
            return worldLength + 16;
        }

        public override byte[] encode(object data, BigEndianBitConverter converter)
        {
            //TODO: World encoding
            return new byte[] {};
        }

        public override object decode(byte[] data, BigEndianBitConverter converter, int offset = 0)
        {
            var height = converter.ToUInt32(data, offset);
            var width = converter.ToUInt32(data, offset+4);
            var worldLength = (int) converter.ToUInt32(data, offset+8);
            var worldWordSize = (int) converter.ToUInt32(data, offset+worldLength+12);

            var worldData = new int[height, width];

            int delta = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int cell = 0;
                    switch (worldWordSize)
                    {
                        case 1:
                            cell = data[offset+delta+12];
                            break;
                        case 2:
                            cell = converter.ToUInt16(data, offset+delta+12);
                            break;
                        default:
                            cell = (int) converter.ToUInt32(data, offset+delta+12);
                            break;
                    }
                    worldData[y, x] = cell;
                    delta += worldWordSize;
                }
            }
            return worldData;
        }

    }
}