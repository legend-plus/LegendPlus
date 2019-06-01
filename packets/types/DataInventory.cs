using LegendDialogue;
using LegendItems;
using MiscUtil.Conversion;
using System.Collections.Generic;

namespace Packets
{
    class DataInventory : DataVarying
    {
        public override int getLength(byte[] data, MiscUtil.Conversion.BigEndianBitConverter converter, int offset = 0)
        {
            return ((int)converter.ToUInt32(data, offset)) + 4;
        }

        public override byte[] encode(object data, BigEndianBitConverter converter)
        {
            Inventory inventory = (Inventory)data;
            //Yes... We're doing this differently from DataOptions
            //This code is an absolute horrid mess
            //And I can only hope that not everything I make in the future builds off this
            //Because I really, really, need to come back to this later once I know what I'm doing.
            //Alyssa 2019-05-25
            //P.S. you have to simplify your substitutions first
            List<byte[]> invData = new List<byte[]>();
            int dataLength = 0;
            foreach (Item item in inventory.items)
            {
                byte[] encoded = item.Encode(converter);
                dataLength += encoded.Length + 4;
                invData.Add(converter.GetBytes(encoded.Length));
                invData.Add(encoded);
            }
            byte[] output = new byte[dataLength + 4];

            System.Buffer.BlockCopy(converter.GetBytes(dataLength), 0, output, 0, 4);
            int offset = 4;
            foreach (byte[] bytes in invData)
            {
                System.Buffer.BlockCopy(bytes, 0, output, offset, bytes.Length);
                offset += bytes.Length;
            }

            return output;
        }

        public override object decode(byte[] data, BigEndianBitConverter converter, int offset = 0)
        {
            Inventory inventory = new Inventory();

            int invLength = (int)converter.ToUInt32(data, offset);
            int pos = offset + 4;

            while (pos < offset + invLength + 4)
            {
                int itemLength = (int)converter.ToUInt32(data, pos);
                pos += 4;
                inventory.AddItem(Item.DecodeItem(data, converter, pos));
                pos += itemLength;
            }
            return inventory;
        }

    }
}