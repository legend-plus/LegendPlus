using MiscUtil.Conversion;

namespace Packets
{
    class DataString : DataVarying
    {
        public override int getLength(byte[] data, MiscUtil.Conversion.BigEndianBitConverter converter, int offset = 0)
        {
            return ((int) converter.ToUInt32(data, offset)) + 4;
        }

        public override byte[] encode(object data, BigEndianBitConverter converter)
        {
            byte[] stringData = System.Text.Encoding.UTF8.GetBytes((string) data);
            byte[] stringLength = converter.GetBytes(stringData.Length);
            byte[] output = new byte[stringData.Length + 4];
            
            System.Buffer.BlockCopy(stringLength, 0, output, 0, 4);
            System.Buffer.BlockCopy(stringData, 0, output, 4, stringData.Length);

            return output;
        }

        public override object decode(byte[] data, BigEndianBitConverter converter, int offset = 0)
        {
            int stringLength = (int) converter.ToUInt32(data, offset);
            string dataString = System.Text.Encoding.UTF8.GetString(data, offset+4, stringLength);
            return dataString;
        }

    }
}