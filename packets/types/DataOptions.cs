using LegendDialogue;
using MiscUtil.Conversion;
using System.Collections.Generic;

namespace Packets
{
    class DataOptions : DataVarying
    {
        public override int getLength(byte[] data, MiscUtil.Conversion.BigEndianBitConverter converter, int offset = 0)
        {
            return ((int)converter.ToUInt32(data, offset)) + 4;
        }

        public override byte[] encode(object data, BigEndianBitConverter converter)
        {
            List<OptionView> optionViews = (List<OptionView>)data;
            List<byte> encodedViews = new List<byte>();
            foreach (OptionView optionView in optionViews)
            {
                optionView.Encode(converter, encodedViews);
            }
            byte[] optionsLength = converter.GetBytes(encodedViews.Count);
            byte[] output = new byte[encodedViews.Count + 4];

            System.Buffer.BlockCopy(optionsLength, 0, output, 0, 4);
            System.Buffer.BlockCopy(encodedViews.ToArray(), 0, output, 4, encodedViews.Count);

            return output;
        }

        public override object decode(byte[] data, BigEndianBitConverter converter, int offset = 0)
        {
            List<OptionView> options = new List<OptionView>();
            int optionsLength = (int)converter.ToUInt32(data, offset);
            int pos = offset+4;
            DataString dataStringDecoder = new DataString();
            DataFixedString dataFixedStringDecoder = new DataFixedString(32);
            while (pos < offset + optionsLength + 4)
            {
                string optionText = (string) dataStringDecoder.decode(data, converter, pos);
                pos += dataStringDecoder.getLength(data, converter, pos);
                string uuid = (string) dataFixedStringDecoder.decode(data, converter, pos);
                pos += dataFixedStringDecoder.dataLength;
                options.Add(new OptionView(optionText, System.Guid.Parse(uuid)));
            }
            return options;
        }

    }
}