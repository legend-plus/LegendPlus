using MiscUtil.Conversion;

namespace Packets
{
    class DataByte : DataType
    {
        public override int length { get{ return 1;} }

        public override object decode(byte[] data, BigEndianBitConverter converter, int offset = 0)
        {
            return (int) data[offset];
        }

        public override byte[] encode(object data, BigEndianBitConverter converter)
        {
            return new byte[] {(byte) ((int) data)};
        }
    }
}