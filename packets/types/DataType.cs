using System.Collections.Generic;

namespace Packets
{
    public abstract class DataType {
        public abstract int length { get; }

        public abstract byte[] encode(object data, MiscUtil.Conversion.BigEndianBitConverter converter);

        public virtual int encodeAsList(object data, MiscUtil.Conversion.BigEndianBitConverter converter, List<byte> output)
        {
            // Only a few types arae actually going to be encoded as lists
            // For convenience sake we can get away with just using the byte arrays and
            // Add them to the list
            byte[] outputArray = encode(data, converter);
            foreach (byte b in outputArray)
            {
                output.Add(b);
            }
            return outputArray.Length;
        }

        public abstract object decode(byte[] data, MiscUtil.Conversion.BigEndianBitConverter converter, int offset = 0);
    }
}
