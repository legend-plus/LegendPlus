using MiscUtil.Conversion;
using Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendDialogue
{
    public class OptionView
    {
        public string text;
        public Guid uuid;

        public static DataType[] schema = {
            new DataString(),
            new DataFixedString(32),
        };

        public OptionView(string text, Guid uuid)
        {
            this.text = text;
            this.uuid = uuid;
        }


        public List<byte> Encode(BigEndianBitConverter converter, List<byte> output = null)
        {
            if (output == null)
            {
                output = new List<byte>();
            }
            output = Packets.Packets.encodeDataAsList(schema, new object[] { text, uuid.ToString("N") }, output);
            return output;
        }
    }
}
