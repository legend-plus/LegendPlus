using LegendItems;
using MiscUtil.Conversion;
using Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendDialogue
{
    public abstract class Substitution
    {
        const short SubId = 0;
        public abstract byte[] Encode(BigEndianBitConverter converter);

        public static Substitution DecodeSubstitution(byte[] data, BigEndianBitConverter converter, int offset = 0)
        {
            //Godot.GD.Print("Decoding from ", BitConverter.ToString(data, offset));
            short subId = converter.ToInt16(data, offset);
            if (subId == 0)
            {
                //Godot.GD.Print(BitConverter.ToString(data, offset));
                DataString dataString = new DataString();
                string text = (string) dataString.decode(data, converter, offset + 2);
                return new StringSubstitution(text);
            }
            else if (subId == 2)
            {
                DataItem dataItem = new DataItem();
                Item item = (Item)dataItem.decode(data, converter, offset + 2);
                return new ItemSubstitution(item);
            }
            else
            {
                return new StringSubstitution("");
            }
        }
    }
}
