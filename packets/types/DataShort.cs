using MiscUtil.Conversion;

namespace Packets
{
    class DataShort : DataType
    {
        public override int length { get{ return 2;} }

        public override object decode(byte[] data, BigEndianBitConverter converter, int offset = 0)
        {
            return converter.ToInt16(data, offset);
        }

        public override byte[] encode(object data, BigEndianBitConverter converter)
        {
            return converter.GetBytes((short) data);
        }
    }
}