namespace Packets
{
    public abstract class DataType {
        public abstract int length { get; }

        public abstract byte[] encode(object data, MiscUtil.Conversion.BigEndianBitConverter converter);

        public abstract object decode(byte[] data, MiscUtil.Conversion.BigEndianBitConverter converter, int offset = 0);
    }
}
