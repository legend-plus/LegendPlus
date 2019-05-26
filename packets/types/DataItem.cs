using LegendItems;
using MiscUtil.Conversion;

namespace Packets
{
    class DataItem : DataVarying
    {
        public override int getLength(byte[] data, MiscUtil.Conversion.BigEndianBitConverter converter, int offset = 0)
        {
            return ((int)converter.ToUInt32(data, offset)) + 4;
        }

        public override byte[] encode(object data, BigEndianBitConverter converter)
        {
            Item item = (Item)data;
            byte[] itemData = item.Encode(converter);
            byte[] itemLength = converter.GetBytes(itemData.Length);

            byte[] output = new byte[itemData.Length + 4];

            System.Buffer.BlockCopy(itemLength, 0, output, 0, 4);
            System.Buffer.BlockCopy(itemData, 0, output, 4, itemData.Length);

            return output;
        }

        public override object decode(byte[] data, BigEndianBitConverter converter, int offset = 0)
        {
            int itemLength = converter.ToInt32(data, offset);
            byte[] itemData = new byte[itemLength];

            System.Buffer.BlockCopy(data, offset + 4, itemData, 0, itemLength);

            return Item.DecodeItem(itemData, converter);
        }

    }
}