using MiscUtil.Conversion;

namespace Packets
{
    class DataInt : DataType
    {
        public override int length { get{ return 4;} }

        public override object decode(byte[] data, BigEndianBitConverter converter, int offset = 0)
        {
            return converter.ToInt32(data, offset);
        }

        public override byte[] encode(object data, BigEndianBitConverter converter)
        {
            return converter.GetBytes((int) data);
        }
    }
}