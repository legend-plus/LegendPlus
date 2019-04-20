using MiscUtil.Conversion;

namespace Packets
{
    class DataFixedString : DataType
    {
        public int dataLength = 0;
        public override int length { get{ return dataLength;} }

        public DataFixedString(int dLength)
        {
            dataLength = dLength;
        }

        public override object decode(byte[] data, BigEndianBitConverter converter, int offset = 0)
        {
            return System.Text.Encoding.UTF8.GetString(data, offset, dataLength);
        }

        public override byte[] encode(object data, BigEndianBitConverter converter)
        {
            return System.Text.Encoding.UTF8.GetBytes((string) data);
        }
    }
}