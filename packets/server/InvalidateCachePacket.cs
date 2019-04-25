namespace Packets
{
    public class InvalidateCachePacket : Packet {
        public override short id { get{ return 10;} }
        public override string name { get{ return "Invalidate Cache";} }

        public static DataType[] schema = {
            new DataFixedString(32)
        };

        public string uuid;

        public InvalidateCachePacket(string entity_uuid )
        {
            uuid = entity_uuid;
        }
        public InvalidateCachePacket(byte[] received_data)
        {
            var decoded = Packets.decodeData(schema, received_data);
            uuid = (string) decoded[0];
        }

        public override byte[] encode()
        {
            var output = Packets.encodeData(schema, new object[] {uuid});
            return output;
        }
    }

}