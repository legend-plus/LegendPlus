using MiscUtil.Conversion;

namespace Packets
{
    class DataEndingString : DataVarying
    {
        public override int getLength(byte[] data, MiscUtil.Conversion.BigEndianBitConverter converter, int offset = 0)
        {
            return data.Length - offset;
        }

        public override byte[] encode(object data, BigEndianBitConverter converter)
        {
            byte[] stringData = System.Text.Encoding.UTF8.GetBytes((string) data);
            return stringData;
        }

        public override object decode(byte[] data, BigEndianBitConverter converter, int offset = 0)
        {
            if (data.Length - offset > 0)
            {
                string dataString = System.Text.Encoding.UTF8.GetString(data, offset, data.Length - offset);
                return dataString;
            }
            else
            {
                return "";
            }
        }

    }
}