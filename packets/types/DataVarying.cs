namespace Packets
{
    public abstract class DataVarying : DataType {
        public override int length { get{ return -1;} }

        public abstract int getLength(byte[] data, MiscUtil.Conversion.BigEndianBitConverter converter, int offset = 0);

    }
}
